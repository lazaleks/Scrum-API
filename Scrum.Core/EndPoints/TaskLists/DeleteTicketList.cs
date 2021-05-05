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

namespace Scrum.Core.EndPoints.TaskLists
{
    public class DeleteTicketList
    {
        public class Command : IRequest<bool>
        {
            public int TicketListId { get; set; }
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
                var ticketList = await _context.TicketLists.Where(x => x.Id == request.TicketListId).FirstOrDefaultAsync();

                if (ticketList == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket list not found.", ErrorCode = 404 });

                _context.TicketLists.Remove(ticketList);
                await _context.SaveChangesAsync();

                return true;
            }
        }

    }
}
