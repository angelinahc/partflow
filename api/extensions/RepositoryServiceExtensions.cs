using api.data.repositories;

namespace api.extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            // Creates a single object for the whole application
            services.AddSingleton<IStationRepository, InMemoryStationRepository>();
            services.AddSingleton<IPartRepository, InMemoryPartRepository>();
            services.AddSingleton<IFlowHistoryRepository, InMemoryFlowHistoryRepository>();

            return services;
        }
    }
}