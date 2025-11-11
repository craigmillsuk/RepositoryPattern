using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace RepositoryPattern.Api.Extensions
{
    public class CosmosDbSettings
    {
        public string Account { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }

    public static class CosmosDbModule
    {
        public static IServiceCollection AddCosmosDbModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDb"));

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
                return new CosmosClient(settings.Account, settings.Key);
            });

            return services;
        }
    }
}
