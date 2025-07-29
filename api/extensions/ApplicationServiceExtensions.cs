using api.services;

namespace api.extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Scoped: Creates a new object for each HTTP request
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IPartService, PartService>();

            return services;
        }
    }
}