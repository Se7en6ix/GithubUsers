using Github.Client.Contracts;
using Github.Client.Model;
using GithubUsers.Shared.HttpClientConfigurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Github.Client.Implementation
{

    public class GithubRepository : IGithubRepository
    {
        private readonly HttpClient _httpClient;
        private readonly GithubConfiguration _githubConfiguration;
        private readonly ILogger<GithubRepository> _logger;

        public GithubRepository(HttpClient httpClient, GithubConfiguration githubConfiguration, ILogger<GithubRepository> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _githubConfiguration = githubConfiguration;
            _logger = logger;
        }

        public async Task<GithubUserMetadata> GetUserAsync(string githubUserName)
        {
            var githubUser = await GetUserTask(githubUserName);
            return githubUser;
        }

        public async Task<IEnumerable<GithubUserMetadata>> GetUsersAsync(params string[] githubUserNames)
        {
            var getUserTasks = githubUserNames.ToList().Select(s => GetUserTask(s.ToLower()));
            var githubUsers = (await Task.WhenAll(getUserTasks)).Where(user => user != null && user.Id > 0);
            return githubUsers;
        }

        public static HttpClient ConfigureHttpClient(HttpClient httpClient, GithubConfiguration githubConfiguration)
        {
            httpClient.BaseAddress = new Uri($"{githubConfiguration.Protocol}{githubConfiguration.BaseUri}");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(githubConfiguration.AcceptHeader));
            httpClient.DefaultRequestHeaders.Add("User-Agent", githubConfiguration.UserAgentHeader);
            return httpClient;
        }

        private async Task<GithubUserMetadata> GetUserTask(string githubUserName)
        {
            string requestUri = $"{_githubConfiguration.UsersEndpoint}{githubUserName.ToLower()}";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                 _logger.LogError($"Failed to fetch Github User {githubUserName}.", null);
                return new GithubUserMetadata();
            }

            var content = await response.Content.ReadAsStringAsync();


            var userMetadata = new GithubUserMetadata();
            if (!string.IsNullOrWhiteSpace(content))
            {
                userMetadata = JsonSerializer.Deserialize<GithubUserMetadata>(content);
                if (userMetadata.Id > 0)
                {
                    _logger.LogInformation($"The Github username {githubUserName}, is owned by {userMetadata.Name}!");
                }
                else
                {
                    _logger.LogWarning($"The Username {githubUserName} does not exist on Github");
                }
            }
            return userMetadata;
        }
    }
}
