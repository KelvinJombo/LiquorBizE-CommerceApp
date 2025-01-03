using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Services to DI Container 

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidateLifetime = true
        };
        options.RequireHttpsMetadata = true;


        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Suppress default behavior
                context.Response.Redirect("/Error?statusCode=401");
                return Task.CompletedTask;
            }
        };

    });

    builder.Services.AddAuthorization(options =>
    {
        // Define a policy for RegularUser
        options.AddPolicy("RegularUserPolicy", policy =>
            policy.RequireRole("RegularUser"));

        // Define a policy for AdminUser
        options.AddPolicy("AdminUserPolicy", policy =>
            policy.RequireRole("AdminUser"));

        // Define a policy for SuperAdminUser
        options.AddPolicy("SuperAdminPolicy", policy =>
            policy.RequireRole("SuperAdminUser"));

        // Define a policy allowing access to both AdminUser and SuperAdminUser
        options.AddPolicy("AdminOrSuperAdminPolicy", policy =>
            policy.RequireRole("AdminUser", "SuperAdminUser"));
    });


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

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapReverseProxy();


app.Run();
