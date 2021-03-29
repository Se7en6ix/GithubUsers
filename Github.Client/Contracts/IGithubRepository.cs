using Github.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Github.Client.Contracts
{
    public interface IGithubRepository
    {
        Task<IEnumerable<GithubUserMetadata>> GetUsersAsync(params string[] githubUserNames);
        Task<GithubUserMetadata> GetUserAsync(string githubUserName);
    }
}
