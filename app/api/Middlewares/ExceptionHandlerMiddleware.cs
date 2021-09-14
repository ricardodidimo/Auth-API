using System;
using System.Threading.Tasks;
using api.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace api.Middlewares
{
    public class ExceptionHandlerAPIMiddleware
    {
            private readonly RequestDelegate _next;

            public ExceptionHandlerAPIMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private async Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
                
                if(ex is DomainException)
                {
                    DomainException handledException = ((DomainException) ex);
                    context.Response.StatusCode = handledException.StatusCode;
                    await context.Response.WriteAsJsonAsync(new APIResponse<string[]>(){
                        StatusCode = handledException.StatusCode,
                        Message = "Request Failure",
                        Data = handledException.ErrorMessages
                    });
                    return;
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new APIResponse<string[]>(){
                    StatusCode = 500,
                    Message = "Unexpected error. Contact us.",
                    Data = new string[]{ex.Message}
                });
                
            }
    }
}