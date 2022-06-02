using System.Collections.Generic;
using WebService.DTO;

namespace Dtos.UserDtos
{
    public class UserForDetailsDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string Role { get; set; }
        public string RegNumber { get; set; }
        public string LastActive { get; set; }
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
        public ICollection<PhotoForDetailsDto> Photos { get; set; }
    }
}
