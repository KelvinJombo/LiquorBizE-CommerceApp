using Odering.Api;
using Odering.Application;
using Odering.Infrastructure;
using Odering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Service to the Container
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

//Configure the Http request Pipeline

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}



app.Run();
