using AutoMapper;
using Github.Client.Contracts;
using Github.Client.Model;
using GithubUsers.Shared;
using GithubUsers.Shared.Services;
using RedisCache.Client.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubUsers.Services
{

    public class GithubUsersService : IGithubUsersService
    {
        private readonly IGithubRepository _githubRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly IMapper _mapper;

        public GithubUsersService(IGithubRepository githubRepository,
                                  ICacheRepository cacheRepository,
                                  IMapper mapper)
        {
            _githubRepository = githubRepository;
            _cacheRepository = cacheRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GithubUser>> GetUsersAsync(params string[] userNames)
        {

            //Get users from Cache
            var cachedUsers = (await GetUsersFromCacheAsync(userNames)).Select(u => { u.FromCache = true; return u; });

            //Filter out Non-Cached users
            var nonCachedUsers = userNames.Select(u => u.ToLower()).Except(cachedUsers.Select(s => s.Login.ToLower())).ToArray();

            //Get Non-Cached users from github
            var githubUsers = (await GetUsersFromGithubAsync(nonCachedUsers)).Select(u => { u.FromCache = false; return u; }); 

            //Create Cache entries for each users fetch from github
            var cacheGithubUsers = await _cacheRepository.SetRecordAsync(githubUsers);

            //combine cached and non cached users
            var users = cachedUsers.ToList().Union(githubUsers).Distinct();

            //return results
            return users ?? new List<GithubUser>().AsEnumerable();
        }


        private async Task<IEnumerable<GithubUser>> GetUsersFromGithubAsync(params string[] userNames)
        {
            var users = _mapper.Map<IEnumerable<GithubUserMetadata>, IEnumerable<GithubUser>>(await _githubRepository.GetUsersAsync(userNames));
            return users ?? new List<GithubUser>().AsEnumerable();
        }

        private async Task<IEnumerable<GithubUser>> GetUsersFromCacheAsync(params string[] userNames)
        {
            var users =await _cacheRepository.GetRecordAsync(userNames);
            return users ?? new List<GithubUser>().AsEnumerable();
        }
    }
}
