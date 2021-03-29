using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubUsers.Shared.Services
{
    public interface IGithubUsersService
    {
        Task<IEnumerable<GithubUser>> GetUsersAsync(params string[] userNames);
    }

}
