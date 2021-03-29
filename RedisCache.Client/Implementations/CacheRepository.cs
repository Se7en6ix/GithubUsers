using GithubUsers.Shared;
using GithubUsers.Shared.HttpClientConfigurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RedisCache.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisCache.Client.Implementations
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;

        private readonly GithubCacheConfiguration _githubCacheConfiguration;

        private readonly ILogger<CacheRepository> _logger;

        public CacheRepository(IDistributedCache cache, GithubCacheConfiguration githubCacheConfiguration, ILogger<CacheRepository> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _githubCacheConfiguration = githubCacheConfiguration ?? throw new ArgumentNullException(nameof(ArgumentNullException)); ;
            _logger = logger;   
        }

        public async Task<GithubUser> GetRecordAsync(string recordId)
        {
            try
            {
                var jsonData = await _cache.GetStringAsync(recordId);
                if (jsonData is null) return default;

                _logger.LogInformation($"Github user '{recordId}' found in Cache");
                return JsonSerializer.Deserialize<GithubUser>(jsonData);
            }
            catch
            {
                _logger.LogError($"Cache threw an error. Make sure it's running!");
                return new GithubUser();
            }
         
        }

        public async Task<IEnumerable<GithubUser>> GetRecordAsync(string[] recordIds)
        {
            var getUserTasks = recordIds.ToList().Select(s => GetRecordAsync(s.ToLower()));
            var githubUsers = (await Task.WhenAll(getUserTasks)).Where(user => user != null && user.Id > 0);
            return githubUsers;
        }

        public async Task<bool> SetRecordAsync(string recordId, GithubUser data)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_githubCacheConfiguration.AbsoluteExpireTime),
                    SlidingExpiration = TimeSpan.FromSeconds(_githubCacheConfiguration.SlidingExpiration)
                };

                await _cache.SetStringAsync(recordId, JsonSerializer.Serialize(data), options);

                _logger.LogInformation($"Github user '{recordId}',has been successfully cached");
                return true;
            }
            catch
            {
                _logger.LogWarning($"Github user '{recordId}', caching failed.");
                return false;
            }
       
        }

        public async Task<bool> SetRecordAsync(IEnumerable<GithubUser> data)
        {
            var setRecordTasks = data.ToList().Select(s => SetRecordAsync(s.Login.ToLower(), s));
            await Task.WhenAll(setRecordTasks);
            return true;
        }
    }
}
