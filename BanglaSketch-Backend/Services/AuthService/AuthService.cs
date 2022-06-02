﻿using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Users;
using Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext context;
        private readonly IMailService mailService;
        private readonly TokenSettings tokenSettings;
        private readonly AppSettings appSettings;
        public AuthService(DataContext context, IMailService mailService, IOptions<TokenSettings> tokenSettings, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.mailService = mailService;
            this.tokenSettings = tokenSettings.Value;
            this.appSettings = appSettings.Value;
        }

        public async Task<ServiceResponse<Tokens>> Login(string email, string password)
        {
            ServiceResponse<Tokens> response = new ServiceResponse<Tokens> { Data = new Tokens() };
            User user = await context.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
            
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
                return response;
            }
            if (user.Username.Equals("admin"))
            {
                response.IsAdmin = true;
            }
            var jwtToken = CreateToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            RemoveOldRefreshTokens(user);
            context.Users.Update(user);
            await context.SaveChangesAsync();

            response.Data.JwtToken = jwtToken;
            response.Data.RefreshToken = refreshToken.Token;
            response.Message = "Login Successful";
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await EmailExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email is not available.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            response.Data = user.Id;
            response.Message = "Registration Successful.";
            return response;
        }

        public async Task<ServiceResponse<Tokens>> RefreshToken(string token)
        {
            ServiceResponse<Tokens> response = new ServiceResponse<Tokens> { Data = new Tokens() };
            if (string.IsNullOrEmpty(token))
            {
                response.Success = false;
                response.Message = "Token is Expired.";
                return response;
            }
            var (refreshToken, user) = GetRefreshToken(token, ref response);
            var newRefreshToken = GenerateRefreshToken();
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            RemoveOldRefreshTokens(user);
            context.Update(user);
            await context.SaveChangesAsync();

            response.Data.JwtToken = CreateToken(user);
            response.Data.RefreshToken = newRefreshToken.Token;
            response.Message = "Refresh token called successfull";
            return response;
        }

        public async Task<ServiceResponse<Tokens>> RevokeToken(string token)
        {
            ServiceResponse<Tokens> response = new ServiceResponse<Tokens> { Data = new Tokens() };
            if (string.IsNullOrEmpty(token))
            {
                response.Success = false;
                response.Message = "Token is required.";
                return response;
            }
            var (refreshToken, user) = GetRefreshToken(token, ref response);
            if (!response.Success)
            {
                return response;
            }
            refreshToken.Revoked = DateTime.UtcNow;
            context.Update(user);
            await context.SaveChangesAsync();

            response.Data.RefreshToken = refreshToken.Token;
            response.Message = $"user is : {user.Id.ToString()}";
            return response;
        }

        public async Task<ServiceResponse<string>> SetOrChangePassword(int userId, string oldPassword, string newPassword)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Something went wrong. User not found.";
                return response;
            }

            else if (oldPassword != null && !VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Old Password doesn't verified.";
                return response;
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            context.Users.Update(user);
            await context.SaveChangesAsync();
            response.Data = user.Id.ToString();
            response.Message = "Password modify successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await context.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "No user found on that Email";
                return response;
            }
            user.ResetToken = RandomTokenString();
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

            context.Users.Update(user);
            await context.SaveChangesAsync();
            SendPasswordResetEmail(user);

            response.Message = "Email sent successfully. Check mail to reset password.";
            return response;
        }

        public async Task<ServiceResponse<string>> ResetPassword(string token, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await context.Users.SingleOrDefaultAsync(user =>
                user.ResetToken == token &&
                user.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                response.Success = false;
                response.Message = $"Token have been expired.";
                return response;
            }

            CreatePasswordHash(password, out byte[] PasswordHash, out byte[] PasswordSalt);
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            user.PasswordReset = DateTime.UtcNow;
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            context.Users.Update(user);
            await context.SaveChangesAsync();
            response.Message = "Password Reset Successfully.";
            return response;
        }

        private async void SendPasswordResetEmail(User user)
        {
            string message;
            var resetUrl = $"{appSettings.AppIISExpressUrl}/v1/auth/reset-password?token={user.ResetToken}";
            message = $@"<p>Please click the below link to reset your password, the link will be invalid after 15 minutes:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

            await mailService.SendEmailAsync(
                toEmail: user.Email,
                subject: "Reset Password",
                body: $@"<h4>Reset Your Password</h4>{message}"
                );
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(tokenSettings.RefreshTokenExpires),
                Created = DateTime.UtcNow
            };
        }

        private (RefreshToken, User) GetRefreshToken(string token, ref ServiceResponse<Tokens> response)
        {
            var user = context.Users.Include(u => u.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User with that token isn't available.";
                return (null, user);
            }
            var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                response.Success = false;
                response.Message = "Invalid Token";
            }
            return (refreshToken, user);
        }

        private void RemoveOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private string RandomTokenString()
        {
            using var rngToken = new RNGCryptoServiceProvider();
            var randomBytes = new Byte[40];
            rngToken.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.TokenSecretKey)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(tokenSettings.JwtTokenExpires),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
