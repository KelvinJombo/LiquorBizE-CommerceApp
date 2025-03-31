using Odering.Api;
using Odering.Application;
using Odering.Infrastructure;
using Odering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Service to the Container

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

//Configure the Http request Pipeline 

var app = builder.Build();

app.UseApiServices();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (UnauthorizedAccessException)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized access. Please log in.");
    }
});


if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}



app.Run();
