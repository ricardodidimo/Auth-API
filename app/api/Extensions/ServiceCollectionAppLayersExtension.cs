using api.Models.Inputs;
using api.Models.Validators;
using api.Repositories;
using api.Services;
using FluentValidation;
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
        public static IServiceCollection AddAppValidatorsLayer(this IServiceCollection services)
        {
            services.AddTransient<IValidator<UserInputModel>, UserInputModelValidator>();
            return services;
        }
    }
}