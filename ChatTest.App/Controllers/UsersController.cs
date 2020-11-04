using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<UserModel>> GetAll([FromHeader(Name = "Authorisation")] string userToken)
        {
            User user = _userService.GetUserByToken(userToken);

            if (user == null || !_userService.IsValid(user))
                return Unauthorized();
            
            IEnumerable<User> users = _userService.GetAll();

            IEnumerable<UserModel> result = _userNameGenerator
                                               .All.Where(n => n != user.Name)
                                               .Select((n, i) => (Name: n
                                                                , Index: i))
                                               .OrderByDescending(t => t.Index)
                                               .Select(t => new UserModel
                                                            {
                                                                Name = t.Name,
                                                                Online = users.FirstOrDefault(u => u.Name == t.Name)?
                                                                            .Online ?? false
                                                            })
                                               .OrderByDescending(u => u.Online);
                
            return Ok(result);
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
