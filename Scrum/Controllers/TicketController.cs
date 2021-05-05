using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrum.Core.EndPoints.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Api.Controllers
{
    [Route("api/users/{userId}/projects/{projectId}/ticketLists/{ticketListId}/tickets")]
    public class TicketController : BaseApiController
    {
        private readonly IMediator _mediator;
        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTickets([FromRoute] int ticketListId)
        {
            var result = await _mediator.Send(new GetAllTickets.Query { TicketListId = ticketListId });
            return Ok(result);
        }
        [HttpGet]
        [Route("{ticketId}")]
        public async Task<IActionResult> GetTicket([FromRoute] int ticketListId, [FromRoute] int ticketId)
        {
            var result = await _mediator.Send(new GetTicket.Query { TicketId = ticketId });
            return Ok(result);
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTicket([FromRoute] int ticketListId, [FromBody] CreateTicket.Command command)
        {
            command.TicketListId = ticketListId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch]
        [Route("{ticketId}")]
        public async Task<IActionResult> EditTicket([FromRoute] int ticketListId, [FromRoute] int ticketId, [FromBody] EditTicket.Command command)
        {
            command.TicketId = ticketId;
            command.TicketListId = ticketListId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{ticketId}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] int ticketId)
        {
            var result = await _mediator.Send(new DeleteTicket.Command { TicketId = ticketId });
            return Ok(result);
        }

        [HttpPatch]
        [Route("{ticketId}/assignTo/{assigneeId}")]
        public async Task<IActionResult> AssignTicket([FromRoute] int ticketId, [FromRoute] int assigneeId)
        {
            var result = await _mediator.Send(new AssignTaskToUser.Command { TaskId = ticketId, UserId = assigneeId });
            return Ok(result);
        }
    }
}
