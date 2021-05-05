using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.DataAccess;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Tasks
{
    public class CreateTicket
    {
        public class Command : IRequest<TicketJson>
        {
            public int TicketListId { get; set; }
            [Required]
            [MaxLength(20)]
            public string Name { get; set; }
            [MaxLength(1000)]
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
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

                if (ticketList.Tickets.Any(x => x.Name == request.Name))
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "There is already a ticket by that name.", ErrorCode = 400 });

                var ticket = new Ticket
                {
                    Name = request.Name,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    CreatedAt = DateTime.Now
                };

                ticketList.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                return _mapper.Map<TicketJson>(ticket);
            }
        }
    }
}
