using Github.Client.Contracts;
using Github.Client.Implementation;
using GithubUsers.Shared.HttpClientConfigurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedisCache.Client.Contracts;
using RedisCache.Client.Implementations;
using StackExchange.Redis;
using System.Net.Http;

namespace GithubUsers.Services
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddGithubHttpClient(this IServiceCollection services, GithubConfiguration configuration)
        {

            services.AddHttpClient(configuration.HttpName, httpClient => httpClient = GithubRepository.ConfigureHttpClient(httpClient, configuration));
            services.AddScoped<IGithubRepository>(p =>
            new GithubRepository(
                p.GetService<IHttpClientFactory>().CreateClient(configuration.HttpName),
                configuration,
                p.GetService<ILogger<GithubRepository>>())
            );
            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, GithubCacheConfiguration configuration)
        {
            var port = 0;
            services.AddStackExchangeRedisCache(o =>
            {          
                o.InstanceName = configuration.InstanceName;
                o.ConfigurationOptions = new ConfigurationOptions()
                {
                    EndPoints = { { configuration.Host, int.TryParse(configuration.Port, out port) ? port : 0 }, },
                    ReconnectRetryPolicy = new LinearRetry(5000),
                    AbortOnConnectFail = false,
                    AllowAdmin = false,
                };

            });

            services.AddScoped<ICacheRepository>(p => new CacheRepository(p.GetService<IDistributedCache>(), configuration, p.GetService<ILogger<CacheRepository>>()));
            return services;
        }

    }
}
