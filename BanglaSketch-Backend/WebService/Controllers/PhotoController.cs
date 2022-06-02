using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repositories;
using WebService.DTO;
using WebService.Helpers;

namespace WebService.Controllers
{
    [Route("api/users/{userId}/photos")]
    [ApiController]
 public class PhotoController : ControllerBase
 {
     private readonly IBanglaSketchRepository _repo;
     private readonly IMapper _mapper;
     private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
     private Cloudinary _cloudinary;

     public PhotoController(IBanglaSketchRepository repo,IMapper mapper,IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
                
            );
            _cloudinary = new Cloudinary(acc);

        }
     [HttpGet("/{id}",Name = "GetPhoto")]
     public async Task<IActionResult> GetPhoto(int id)
     {

         var photoFormRepo = await _repo.GetPhoto(id);
         var photoToReturn = _mapper.Map<PhotoToReturnDto>(photoFormRepo);
         return Ok(photoToReturn);
     }
        [HttpPost]
        public async Task<IActionResult> AddPhotoforUser(int userId,[FromForm] UserPhotoCreationDto userPhotoCreationDto)
        {
            /*if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();*/
            var userFromRepo = await _repo.GetUser(userId);
            var file = userPhotoCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length >= 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Gravity("face").Crop("fill")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            userPhotoCreationDto.Url = uploadResult.Url.ToString();
                userPhotoCreationDto.PublicId = uploadResult.PublicId;
                var photo = _mapper.Map<Photo>(userPhotoCreationDto);
                if (!userFromRepo.Photos.Any(u => u.IsMain))
                {
                    photo.IsMain = true;
                }
                userFromRepo.Photos.Add(photo);
                if (await _repo.SaveAll())
                {
                    var photoToReturn = _mapper.Map<PhotoToReturnDto>(photo);
                    return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
                }
                //string imageName = null;
                /*var httpRequest = HttpContext.Current.Request;
                //Upload Image
                var postedFile = httpRequest.Files["Image"];
                //Create custom filename
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);*/
                return BadRequest("couldNot Add photo");
            }
            
        
    }
}