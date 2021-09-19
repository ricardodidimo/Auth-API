using System.Collections.Generic;
using System.Linq;
using api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ModelBindingHandler(this IServiceCollection services, ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                context.HttpContext.Response.StatusCode = 400;
                
                return new JsonResult(new APIResponse<List<string>>()
                    {
                        StatusCode = 400,
                        Message = "Validation failure",
                        Data = context.ModelState
                            .SelectMany(ms => ms.Value.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    }
                );

            };

            return services;
        }
    }
}