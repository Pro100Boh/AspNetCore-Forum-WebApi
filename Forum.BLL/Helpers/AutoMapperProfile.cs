using AutoMapper;
using Forum.BLL.DTO;
using Forum.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
