using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.Exceptions;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Tasks
{
    public class DeleteTicket
    {
        public class Command : IRequest<bool>
        {
            public int TicketId { get; set; }
        }
        public class Handler : IRequestHandler<Command,bool>
        {
            private readonly ScrumContext _context;

            public Handler(ScrumContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var ticket = await _context.Tickets.Where(x => x.Id == request.TicketId).FirstOrDefaultAsync();

                if (ticket == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket not found.", ErrorCode = 404 });

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
