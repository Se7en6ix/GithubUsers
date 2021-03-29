using AutoMapper;
using GithubUsers.Api.Models;
using GithubUsers.Shared;
using GithubUsers.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GithubUsers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubUsersController : ControllerBase
    {
        private readonly ILogger<GithubUsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IGithubUsersService _userService;

        public GithubUsersController(IGithubUsersService service, IMapper mapper, ILogger<GithubUsersController> logger)
        {
            _userService = service;
            _mapper = mapper;
            _logger = logger; 
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(IEnumerable<GithubDtoUserResponse>), (int)HttpStatusCode.OK)]     

        public async Task<IActionResult> Users([FromBody] List<string> userNames)
        {
            var users = _mapper.Map<IEnumerable<GithubUser>, IEnumerable<GithubDtoUserResponse>>(await _userService.GetUsersAsync(userNames.ToArray()));
            _logger.LogInformation("API called");
            return Ok(users.OrderBy(user => user.Name));
        }
    }
}
