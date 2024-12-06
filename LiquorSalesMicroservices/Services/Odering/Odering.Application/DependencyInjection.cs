using BuildingBlocks.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Odering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof (LoggingBehavior<,>));
            });
                 return services;
        }


    }
}
