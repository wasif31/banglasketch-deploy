using System.Linq;
using AutoMapper;
using Dtos.UserDtos;
using Model;
using Model.Users;
using WebService.DTO;

namespace WebService.Helpers
{
    public class AutoMapperProfilesHelper:Profile
    {
        public AutoMapperProfilesHelper()
        {
            CreateMap<User, UserForDetailsDto>()
                /*.ForMember(dest=>dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
                })*/
                ;
            CreateMap<User, UserForListDto>()
                /*.ForMember(dest=>dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
                })*/
                ;
            CreateMap<Photo, PhotoForDetailsDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoToReturnDto>();
            CreateMap<UserPhotoCreationDto, Photo>();
        }
    }
}