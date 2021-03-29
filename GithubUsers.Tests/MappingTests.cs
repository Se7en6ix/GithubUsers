using AutoMapper;
using Github.Client.Model;
using GithubUsers.Api;
using GithubUsers.Api.Models;
using GithubUsers.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace GithubUsers.Tests
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void Ctor_MappingExistsFor_GithubUserMetadata_To_GithubUser()
        {
            var config = (MapperConfiguration)CreateDefaultMapper().ConfigurationProvider;
            var typeMap = config.FindTypeMapFor<GithubUserMetadata, GithubUser>();
            Assert.IsNotNull(typeMap);
        }

        [TestMethod]
        public void Ctor_MappingExistsFor_GithubUser_To_GithubDtoUserResponse()
        {
            var config = (MapperConfiguration)CreateDefaultMapper().ConfigurationProvider;
            var typeMap = config.FindTypeMapFor<GithubUser, GithubDtoUserResponse>();
            Assert.IsNotNull(typeMap);
        }

        [TestMethod]
        public void Map_ConvertsCorrectly_GithubUserMetadata_To_GithubUserl()
        {
            var sourceModel = new GithubUserMetadata
            {
                Id = 47313,
                Login = "fabpot",
                Name = "Fabien Potencier",
                Company = "Symfony/Blackfire",
                Followers = 11319,
                PublicRepositories = 55,
            };

            var mapper = CreateDefaultMapper();
            var destionationModel = mapper.Map<GithubUser>(sourceModel);

            Assert.IsNotNull(destionationModel);
            Assert.AreEqual(sourceModel.Id, destionationModel.Id);
            Assert.AreEqual(sourceModel.Login, destionationModel.Login);
            Assert.AreEqual(sourceModel.Name, destionationModel.Name);
            Assert.AreEqual(sourceModel.Company, destionationModel.Company);
            Assert.AreEqual(sourceModel.Followers, destionationModel.Followers);
            Assert.AreEqual(sourceModel.PublicRepositories, destionationModel.PublicRepositories);
        }

        [TestMethod]
        public void Map_ConvertsCorrectly__To_GithubUser_To_GithubDtoUserResponse()
        {
            var sourceModel = new GithubUser
            {
                Id = 47313,
                Login = "fabpot",
                Name = "Fabien Potencier",
                Company = "Symfony/Blackfire",
                Followers = 11319,
                PublicRepositories = 55,
            };

            var mapper = CreateDefaultMapper();
            var destionationModel = mapper.Map<GithubDtoUserResponse>(sourceModel);


            Assert.IsNotNull(destionationModel);
            Assert.AreEqual(sourceModel.Id, destionationModel.Id);
            Assert.AreEqual(sourceModel.Login, destionationModel.Login);
            Assert.AreEqual(sourceModel.Name, destionationModel.Name);
            Assert.AreEqual(sourceModel.Company, destionationModel.Company);
            Assert.AreEqual(sourceModel.Followers, destionationModel.Followers);
            Assert.AreEqual(sourceModel.PublicRepositories, destionationModel.PublicRepositories);

        }


        private static IMapper CreateDefaultMapper()
        {
            var configuration = new MapperConfiguration(x =>
            {
                x.AddMaps(
                 Assembly.GetAssembly(typeof(Startup)),
                 Assembly.GetAssembly(typeof(Services.MappingProfile)));
            });
            return new Mapper(configuration);
        }
    }
}
