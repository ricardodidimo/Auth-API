using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace api.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            
            context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));
            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));

            context.Response.Headers.Add("Content-Security-Policy", "default-src:'self'");
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));
            context.Response.Headers.Add("server", "none");
            return _next(context);
        }
    }
}