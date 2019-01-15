using Forum.DAL.Interfaces;
using Forum.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.DAL.Helpers
{
    public static class DIForDAL
    {
        public static IServiceCollection AddDIForDAL(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();

            return services;
        }
    }
}
