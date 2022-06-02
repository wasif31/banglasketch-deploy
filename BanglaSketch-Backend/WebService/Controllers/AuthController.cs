using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dtos;
using Dtos.Responses;
using Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Users;
using Services;
using Services.AuthService;
using Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Model;

using WebService.DTO;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMailService mailService;
        private readonly TokenSettings tokenSettings;
        //private readonly IAuthRepository repo;
        //private readonly IConfiguration config;
        public AuthController(IAuthService authService, IMailService mailService,  IOptions<TokenSettings> tokenSettings)
        {
          //  repo = _repo;
           // config = _config;
            this.authService = authService;
            this.mailService = mailService;
            this.tokenSettings = tokenSettings.Value;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMail(MailRequestDto req)
        {
            try
            {
                await mailService.SendEmailAsync(req.Mail, req.Subject, req.Body);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<int> response = await authService.Register(
                new User { Username = request.UserName, Email = request.Email }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<Tokens> response = await authService.Login(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            SetTokenToCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            ServiceResponse<Tokens> response = await authService.RefreshToken(refreshToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            SetTokenToCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        [HttpPost("Revoke-Token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenDto request)
        {
            var token = request.Token ?? Request.Cookies["refreshToken"];
            ServiceResponse<Tokens> response = await authService.RevokeToken(token);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            ServiceResponse<string> response = await authService.ForgotPassword(request.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            ServiceResponse<string> response = await authService.ResetPassword(request.Token, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("Set-Password")]
        public async Task<IActionResult> SetPassword(SetPasswordDto request)
        {
            int id = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            ServiceResponse<string> response = await authService.SetOrChangePassword(userId: id, oldPassword: null, newPassword: request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("Change-Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            int id = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            ServiceResponse<string> response = await authService.SetOrChangePassword(userId: id, oldPassword: request.OldPassword, newPassword: request.NewPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        public void SetTokenToCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(tokenSettings.RefreshTokenExpires)
            };
            Response.Cookies.Append(
                "refreshToken", token, cookieOptions);
        }
        /*[HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            userRegister.UserName = userRegister.UserName.ToLower();
            if (await repo.UserExists(userRegister.UserName))
                return BadRequest("Username already taken");
            var UserToCreate = new User
            {
                UserName = userRegister.UserName
            };
            var createdUser = await repo.Register(UserToCreate, userRegister.Email, userRegister.Password, userRegister.Role, userRegister.RegNumber);
            return StatusCode(201);
        }*/
        /*[HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var userFromRepo = await repo.Login(userLogin.UserName.ToLower() , userLogin.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });*/
       // }
    }
}
