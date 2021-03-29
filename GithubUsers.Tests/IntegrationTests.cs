using Github.Client.Implementation;
using GithubUsers.Shared;
using GithubUsers.Shared.HttpClientConfigurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisCache.Client.Implementations;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GithubUsers.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private readonly GithubConfiguration _mockedGithubConfiguration = new GithubConfiguration
        {
            HttpName = "GithubUsersHttpClient",
            Protocol = "https://",
            BaseUri = "api.github.com",
            UsersEndpoint = "/users/",
            UserAgentHeader = "GetUserFromGitHub",
            AcceptHeader = "application/vnd.github.v3+json"
        };

        private readonly GithubCacheConfiguration _mockedGithubCacheConfiguration = new GithubCacheConfiguration
        {
            Host = "localhost",
            Port = "5009",
            InstanceName = "solo_",
            AbsoluteExpireTime = 120,
            SlidingExpiration= 120,
        };


        [TestMethod]
        public async Task AbleToInsertAndReadDataFromCache()
        {
            var services = new ServiceCollection();

            var port = 0;

            services.AddStackExchangeRedisCache(o =>
            {
                o.InstanceName = _mockedGithubCacheConfiguration.InstanceName;
                o.ConfigurationOptions = new ConfigurationOptions()
                {
                    EndPoints = { { _mockedGithubCacheConfiguration.Host, int.TryParse(_mockedGithubCacheConfiguration.Port, out port) ? port : 0 }, },
                    ReconnectRetryPolicy = new LinearRetry(5000),
                    AbortOnConnectFail = false,
                    AllowAdmin = false,
                };

            });

            var provider = services.BuildServiceProvider();
            var sut = new CacheRepository(provider.GetService<IDistributedCache>(), _mockedGithubCacheConfiguration, new NullLogger<CacheRepository>());


            var user = new GithubUser
            {
                Id = 47313,
                Login = "fabpot",
                Name = "Fabien Potencier",
                Company = "Symfony/Blackfire",
                Followers = 11319,
                PublicRepositories = 55,
            };
            await sut.SetRecordAsync(user.Login, user);

            var dataFromCache = await sut.GetRecordAsync("fabpot");
            Assert.IsNotNull(dataFromCache);
            Assert.AreEqual(user.Id, dataFromCache.Id);
        }

        [TestMethod]

        public async Task AbleToGetUsersFromGithub()
        {
            var users = new string[] { "ahejlsberg", "timcorey", "julielerman" };

            var httpClient = ConfigureHttpClient(new HttpClient(), _mockedGithubConfiguration);
            var sut = new GithubRepository(httpClient, _mockedGithubConfiguration, new NullLogger<GithubRepository>());

            var result = (await sut.GetUsersAsync(users)).OrderBy(o => o.Name).ToArray();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(users[0].ToLower(), result[0].Login.ToLower());
            Assert.AreEqual(users[1].ToLower(), result[2].Login.ToLower());
            Assert.AreEqual(users[2].ToLower(), result[1].Login.ToLower());
        }

        public HttpClient ConfigureHttpClient(HttpClient httpClient, GithubConfiguration githubConfiguration)
        {
            httpClient.BaseAddress = new Uri($"{githubConfiguration.Protocol}{githubConfiguration.BaseUri}");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(githubConfiguration.AcceptHeader));
            httpClient.DefaultRequestHeaders.Add("User-Agent", githubConfiguration.UserAgentHeader);
            return httpClient;
        }

        public class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly string _response;
            private readonly HttpStatusCode _statusCode;

            public string Input { get; private set; }
            public int NumberOfCalls { get; private set; }

            public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
            {
                _response = response;
                _statusCode = statusCode;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                NumberOfCalls++;
                if (request.Content != null) // Could be a GET-request without a body
                {
                    Input = await request.Content.ReadAsStringAsync();
                }
                return new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent(_response)
                };
            }
        }

    }
}