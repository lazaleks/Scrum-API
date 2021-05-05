using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrum.Core.EndPoints.TaskLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Api.Controllers
{
    [Route("api/users/{userId}/projects/{projectId}/ticketLists")]
    public class TicketListController : BaseApiController
    {
        private readonly IMediator _mediator;

        public TicketListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTicketLists([FromRoute] int projectId)
        {
            var result = await _mediator.Send(new GetAllTicketLists.Query { ProjectId = projectId });
            return Ok(result);
        }
        [HttpGet]
        [Route("{ticketListId}")]
        public async Task<IActionResult> GetTicketList([FromRoute] int projectId, [FromRoute] int ticketListId)
        {
            var result = await _mediator.Send(new GetTicketList.Query { ProjectId = projectId, TicketListId = ticketListId });
            return Ok(result);
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTicketList([FromRoute] int projectId,[FromBody] CreateTicketList.Command command)
        {
            command.ProjectId = projectId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch]
        [Route("{ticketListId}")]
        public async Task<IActionResult> EditTicketList([FromRoute] int projectId, [FromRoute] int ticketListId, [FromBody] EditTicketList.Command command)
        {
            command.ProjectId = projectId;
            command.TicketListId = ticketListId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{ticketListId}")]
        public async Task<IActionResult> DeleteTicketList([FromRoute] int ticketListId)
        {
            var result = await _mediator.Send(new DeleteTicketList.Command { TicketListId = ticketListId });
            return Ok(result);
        }
    }
}
