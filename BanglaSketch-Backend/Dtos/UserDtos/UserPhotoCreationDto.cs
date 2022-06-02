using System;
using Microsoft.AspNetCore.Http;
namespace Dtos.UserDtos
{
    public class UserPhotoCreationDto
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        
        
        public bool IsMain { get; set; }
        public UserPhotoCreationDto()
        {
            DateAdded=DateTime.Now;
       
        }
    }
}