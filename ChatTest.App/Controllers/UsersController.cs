using System;
using System.ComponentModel.DataAnnotations;
using ChatTest.App.Models;
using ChatTest.App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatTest.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserNameGenerator _userNameGenerator;

        public UsersController(IUserService userService, 
                               ITokenGenerator tokenGenerator, 
                               IUserNameGenerator userNameGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
            _userNameGenerator = userNameGenerator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Register([FromBody, Required] CliendId client)
        {
            if (client == null)
                return BadRequest("Missing clientId");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userName = _userNameGenerator.Generate();
                var token = _tokenGenerator.Generate(userName, client);
                var user = new User
                {
                    Name = userName,
                    ConnectionId = client.ConnectionId,
                    Token = token
                };

                _userService.Register(user);

                return Created("", user);
            }
            catch(InvalidOperationException)
            {
                return Problem("No username available");
            }
        }

        [HttpPut("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Update([FromRoute] string userName,
                                         [FromBody, Required] CliendId client)
        {
            if (client == null)
                return BadRequest("Missing clientId");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = _tokenGenerator.Generate(userName, client);
                var user = new User
                           {
                               Name = userName,
                               ConnectionId = client.ConnectionId,
                               Token = token
                           };

                _userService.Register(user);

                return Ok(user);
            }
            catch(InvalidOperationException)
            {
                return Problem("No username available");
            }
        }
    }
}
