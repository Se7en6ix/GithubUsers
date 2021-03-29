using System;
using System.Collections.Generic;
using System.Text;

namespace GithubUsers.Shared.HttpClientConfigurations
{
    public class GithubConfiguration
    {
        public string HttpName { get; set; }
        public string Protocol { get; set; }
        public string BaseUri { get; set; }
        public string UsersEndpoint { get; set; }        
        public string AcceptHeader { get; set; }
        public string UserAgentHeader { get; set; }

    }
}
