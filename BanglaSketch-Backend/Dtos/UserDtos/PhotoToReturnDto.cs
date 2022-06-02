using System;

namespace Dtos.UserDtos
{
    public class PhotoToReturnDto
    {
        
        public int Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
    
}