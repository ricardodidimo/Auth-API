using api.Extensions;
using api.Middlewares;
using api.Models.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddFastEndpoints();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

string dbConn = config.GetValue<string>("DbConn");
builder.Services.AddDbContext<AppDbContext>(config => config.UseNpgsql(dbConn));

builder.Services.AddSwaggerDoc();
builder.Services.AddAuthenticationConfig(config);
//builder.Services.Configure<ApiBehaviorOptions>(o => ervices.ModelBindingHandler(o));

builder.Services.AddAppServicesLayer();
builder.Services.AddAppRepositoriesLayer();
builder.Services.AddAppValidatorsLayer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3();

app.UseMiddleware<ExceptionHandlerAPIMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseHttpsRedirection();
app.UseHsts();

app.Run();