namespace Odering.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {

            return services;
        }


        public static WebApplication UseApiServices(this WebApplication webApp)
        {

            return webApp;
        }

    }
}
