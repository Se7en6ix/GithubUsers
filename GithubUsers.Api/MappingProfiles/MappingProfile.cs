using AutoMapper;
using GithubUsers.Api.Models;
using GithubUsers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubUsers.Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GithubUser, GithubDtoUserResponse>();
        }
    }
}
