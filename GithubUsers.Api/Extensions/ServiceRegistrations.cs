using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GithubUsers.Api.Extensions
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddCrossCuttingDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(factory =>
            {
                var config = new MapperConfiguration(profiles=>
                {
                    profiles.AddMaps(
                     Assembly.GetAssembly(typeof(Startup)),
                     Assembly.GetAssembly(typeof(Services.MappingProfile)));
                }); 

                return new Mapper(config);
            });
            return services;
        }
    }
}
