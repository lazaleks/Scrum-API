using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrum.Core.EndPoints.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Api.Controllers
{
    [Route("api/users/{userId}/projects")]
    public class ProjectController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProject([FromRoute] int userId, [FromBody] CreateProject.Command command)
        {
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProjects([FromRoute] int userId)
        {
            var result = await _mediator.Send(new GetAllProjects.Query { UserId = userId });
            return Ok(result);
        }
        [HttpGet]
        [Route("{projectId}")]
        public async Task<IActionResult> GetProject([FromRoute] int userId, [FromRoute] int projectId)
        {
            var result = await _mediator.Send(new GetProject.Query { ProjectId = projectId, UserId = userId });
            return Ok(result);
        }

        [HttpDelete]
        [Route("{projectId}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int userId, [FromRoute] int projectId)
        {
            var result = await _mediator.Send(new DeleteProject.Command { ProjectId = projectId, UserId = userId });
            return Ok(result);
        }

        [HttpPatch]
        [Route("{projectId}")]
        public async Task<IActionResult> EditProject([FromRoute] int userId, [FromRoute] int projectId, [FromBody] EditProject.Command command)
        {
            command.UserId = userId;
            command.ProjectId = projectId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch]
        [Route("{projectId}/assignProject/{assigneeId}")]
        public async Task<IActionResult> AssignProject([FromRoute] int userId,[FromRoute] int assigneeId,[FromRoute] int projectId)
        {
            var result = await _mediator.Send(new AssignProject.Command { UserId = userId, AssigneeId = assigneeId, ProjectId = projectId });
            return Ok(result);
        }
    }
}
