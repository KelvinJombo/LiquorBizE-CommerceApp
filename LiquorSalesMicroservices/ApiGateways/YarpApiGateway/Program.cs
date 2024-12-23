//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
 
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Services to DI Container 

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
//        options.Audience = builder.Configuration["Auth0:Audience"];
//    });

builder.Services.AddAuthorization();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

 
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("customPolicy", policy =>
//        policy.RequireAuthenticatedUser());
//});

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

// Configure the HTTP Request Pipeline

//app.UseAuthentication();

//app.UseAuthorization();

app.UseRateLimiter();

app.MapReverseProxy();


app.Run();
