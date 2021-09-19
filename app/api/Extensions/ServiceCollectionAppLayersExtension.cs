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
        /// <summary>Extension for registering 'services' interfaces and their implementation in the dependency injection container</summary>
        public static IServiceCollection AddAppServicesLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        /// <summary>Extension for registering 'repositories' interfaces and their implementation in the dependency injection container</summary>
        public static IServiceCollection AddAppRepositoriesLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
        /// <summary>Extension for registering 'validators' interfaces and their implementation in the dependency injection container</summary>
        public static IServiceCollection AddAppValidatorsLayer(this IServiceCollection services)
        {
            services.AddTransient<IValidator<UserInputModel>, UserInputModelValidator>();
            return services;
        }
    }
}