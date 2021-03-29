namespace GithubUsers.Shared.HttpClientConfigurations
{
    public class GithubCacheConfiguration
    {
        public string InstanceName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public int AbsoluteExpireTime { get; set; }
        public int SlidingExpiration { get; set; }
        public string UserAgentHeader { get; set; }

    }
}
