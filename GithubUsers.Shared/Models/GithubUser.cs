using System;
using System.Text.Json.Serialization;

namespace GithubUsers.Shared
{
    public class GithubUser
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public long Followers { get; set; }

        public long PublicRepositories { get; set; }

        public long AverageFollowers
        {
            get
            {
                return PublicRepositories == 0 ? 0 : Followers / PublicRepositories;
            }
        }

        public bool FromCache { get; set; }
    }
}
