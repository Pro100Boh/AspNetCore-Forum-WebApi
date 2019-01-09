using Forum.BLL.Interfaces;
using Forum.BLL.Services;
using Forum.DAL.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Helpers
{
    public static class DIForBLL
    {
        public static IServiceCollection AddDIForBLL(this IServiceCollection services)
        {
            services.AddDIForDAL();

            services.AddTransient<IPostsService, PostsService>();

            services.AddTransient<IUsersService, UsersService>();

            return services;
        }
    }
}
