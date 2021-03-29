using AutoMapper;
using Github.Client.Model;
using GithubUsers.Shared;

namespace GithubUsers.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GithubUserMetadata, GithubUser>();
        }
    }
}
