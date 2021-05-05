using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Tasks
{
    public class EditTicket
    {
        public class Command : IRequest<TicketJson>
        {
            public int TicketId { get; set; }
            public int TicketListId{ get; set; }
            [MaxLength(20)]
            public string Name { get; set; }
            [MaxLength(1000)]
            public string Description { get; set; }
            public bool? Done { get; set; }
        }
        public class Handler : IRequestHandler<Command,TicketJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TicketJson> Handle(Command request, CancellationToken cancellationToken)
            {

                var ticketList = await _context.TicketLists.Where(x => x.Id == request.TicketListId)
                    .Include(x => x.Tickets)
                    .FirstOrDefaultAsync();

                if (ticketList == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket list not found.", ErrorCode = 404 });

                var ticket = ticketList.Tickets.Where(x => x.Id == request.TicketId).FirstOrDefault();

                if (ticket == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket not found.", ErrorCode = 404 });

                if (request.Done.HasValue)
                {
                    ticket.Done = request.Done.Value;
                    if (request.Done.Value == true)
                        ticket.CompletedAt = DateTime.Now;
                }
                if (!String.IsNullOrEmpty(request.Name) && !String.IsNullOrWhiteSpace(request.Name))
                {
                    if (ticketList.Tickets.Any(x => x.Name == request.Name))
                        throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "There is already a ticket by that name.", ErrorCode = 400 });
                    ticket.Name = request.Name;
                }
                if (!String.IsNullOrEmpty(request.Description))
                {
                    ticket.Description = request.Description;
                }

                await _context.SaveChangesAsync();
                return _mapper.Map<TicketJson>(ticket);
            }
                
        }
    }
}
