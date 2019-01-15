using AutoMapper;
using Forum.API.Models;
using Forum.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*
            CreateMap<PostDTO, PostView>();
            CreateMap<PostView, PostDTO>();
            */
            CreateMap<RegistrationView, UserDTO>();
            CreateMap<UserUpdateView, UserDTO>().
                ForMember(dest => dest.Username,
                          opts => opts.MapFrom(src => src.NewUsername));

        }
    }
}
