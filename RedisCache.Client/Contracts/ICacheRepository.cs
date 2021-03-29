using GithubUsers.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisCache.Client.Contracts
{
    public interface ICacheRepository
    {
        Task<bool> SetRecordAsync(string recordId, GithubUser data);
        Task<bool> SetRecordAsync(IEnumerable<GithubUser> data);
        Task<GithubUser> GetRecordAsync(string recordId);

        Task<IEnumerable<GithubUser>> GetRecordAsync(string[] recordIds);
    }
}
