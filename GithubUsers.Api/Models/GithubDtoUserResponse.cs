using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubUsers.Api.Models
{
    public class GithubDtoUserResponse
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public long Followers { get; set; }

        public long PublicRepositories { get; set; }

        public long AverageFollowers { get; set; }


        //uncomment this property to identify if teh result is from cache        
        //public bool FromCache { get; set; }
    }
}
