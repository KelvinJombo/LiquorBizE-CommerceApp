using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Odering.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter();
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration["Auth0:Domain"]}";
                options.Audience = configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://{configuration["Auth0:Domain"]}",
                    ValidateAudience = true,
                    ValidAudience = configuration["Auth0:Audience"],
                    ValidateLifetime = true
                };
                options.RequireHttpsMetadata = true;

            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("RegularUserPolicy", policy =>
                    policy.RequireRole("RegularUser"));

                options.AddPolicy("AdminUserPolicy", policy =>
                    policy.RequireRole("AdminUser"));

                options.AddPolicy("SuperAdminPolicy", policy =>
                    policy.RequireRole("SuperAdminUser"));

                options.AddPolicy("AdminOrSuperAdminPolicy", policy =>
                    policy.RequireRole("AdminUser", "SuperAdminUser"));
                options.AddPolicy("CustomPolicy", policy =>
                    policy.RequireClaim("custom-claim", "value"));
            });



            return services;
        }


        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            return app;
        }

    }
}
