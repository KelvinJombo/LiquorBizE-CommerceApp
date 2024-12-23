using Microsoft.AspNetCore.Authentication.JwtBearer;
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

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}



app.Run();
