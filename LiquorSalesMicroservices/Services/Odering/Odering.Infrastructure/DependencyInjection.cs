using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odering.Infrastructure.Data.Interceptors;

namespace Odering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.AddInterceptors(new AuditableEntityInterceptor());
                options.UseSqlServer(connectionString);
            });
           

            return services;
        }
    }
}
