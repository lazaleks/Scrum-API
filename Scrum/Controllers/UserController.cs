using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrum.Core.EndPoints.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsers.Query { });
            return Ok(result);
        }

        [HttpPost]
        [Route("api/users/login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch]
        [Route("api/users")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/users/{userId}/myTasks")]
        public async Task<IActionResult> GetTicketsForUser([FromRoute] int userId)
        {
            var result = await _mediator.Send(new GetTicketsForUser.Query { UserId = userId });
            return Ok(result);
        }
    }
}
