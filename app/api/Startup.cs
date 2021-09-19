using api.Extensions;
using api.Middlewares;
using api.Models.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<AppDbContext>(config => config.UseNpgsql(Configuration["DbConn"]));
            
            services.AddSwaggerConfig();
            services.AddAuthenticationConfig(Configuration);
            services.Configure<ApiBehaviorOptions>(o => services.ModelBindingHandler(o));
            
            services.AddAppServicesLayer();
            services.AddAppRepositoriesLayer();
            services.AddAppValidatorsLayer();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
         
            app.UseMiddleware<ExceptionHandlerAPIMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
