using AutoMapper;
using GithubUsers.Api.Extensions;
using GithubUsers.Api.Middleware;
using GithubUsers.Services;
using GithubUsers.Shared.HttpClientConfigurations;
using GithubUsers.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net.Http;

namespace GithubUsers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var githubSettings = Configuration.GetSection("GithubClient").Get<GithubConfiguration>();
            var cacheSettings = Configuration.GetSection("RedisCacheClient").Get<GithubCacheConfiguration>();
            services.Configure<GithubConfiguration>(Configuration.GetSection("GithubClient"));

            services.AddControllers();
            services
                .AddGithubHttpClient(githubSettings)
                .AddCache(cacheSettings)
                .AddCrossCuttingDependencies();
            services.AddScoped<IGithubUsersService, GithubUsersService>();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Github User Search", 
                    Version = "v1", 
                    Description = "A net core Webapi project that has an API endpoint that takes a list of github usernames and returns to the user a list of basic information"
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = new JsonExceptionMiddleware().Invoke
                });
            }
              

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Go Rooms V1");
            });
        }
    }
}
