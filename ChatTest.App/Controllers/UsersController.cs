using System;
using System.ComponentModel.DataAnnotations;
using ChatTest.App.Models;
using ChatTest.App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ChatTest.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserNameGenerator _userNameGenerator;

        public UsersController(IMemoryCache cache, ITokenGenerator tokenGenerator, IUserNameGenerator userNameGenerator)
        {
            _cache = cache;
            _tokenGenerator = tokenGenerator;
            _userNameGenerator = userNameGenerator;
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromBody, Required] CliendId client)
        {
            if (client == null)
                return BadRequest("Missing clientId");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userName = _userNameGenerator.Generate();
                var token = _tokenGenerator.Generate(userName, client.Id);
                var user = new User
                {
                    Name = userName,
                    Token = token
                };

                _cache.Set(userName, user);

                return Ok(user);
            }
            catch(InvalidOperationException)
            {
                return Problem("No username available");
            }
        }
    }
}
