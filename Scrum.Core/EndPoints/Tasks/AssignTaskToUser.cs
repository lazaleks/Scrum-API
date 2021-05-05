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
    public class AssignTaskToUser
    {
        public class Command : IRequest<bool>
        {
            public int TaskId { get; set; }
            public int UserId { get; set; }
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
                var user = await _context.Users.AsNoTracking()
                    .Where(x => x.Id == request.UserId)
                    .Include(x => x.User_Projects)
                    .FirstOrDefaultAsync();
                if (user == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "User not found.", ErrorCode = 404 });

                var ticket = await _context.Tickets.Where(x => x.Id == request.TaskId)
                    .Include(x => x.TicketList)
                    .FirstOrDefaultAsync();

                if (ticket == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket not found.", ErrorCode = 404 });
                var projectId = ticket.TicketList.ProjectId;

                if (!user.User_Projects.Any(x => x.ProjectId == projectId))
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "That user does not have access to that project.", ErrorCode = 400 });

                ticket.AssignedTo = user.Id;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
