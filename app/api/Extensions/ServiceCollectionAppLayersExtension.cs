using api.Repositories;
using api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions
{
    public static class ServiceCollectionAppLayersExtension
    {
        public static IServiceCollection AddAppServicesLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
        public static IServiceCollection AddAppRepositoriesLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}