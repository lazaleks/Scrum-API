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

namespace Scrum.Core.EndPoints.Projects
{
    public class AssignProject
    {
        public class Command : IRequest<bool>
        {
            public int UserId { get; set; }
            public int AssigneeId { get; set; }
            public int ProjectId { get; set; }
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
                var user = await _context.Users.Where(x => x.Id == request.UserId)
                    .Include(x => x.User_Projects)
                    .ThenInclude(x => x.Project)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "User not found.", ErrorCode = 404 });

                var projects = user.User_Projects.Select(x => x.Project).ToList();

                var project = projects.Where(x => x.Id == request.ProjectId).FirstOrDefault();
                if (project == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project not found.", ErrorCode = 404 });

                var assignee = await _context.Users.Where(x => x.Id == request.AssigneeId)
                    .Include(x => x.User_Projects)
                    .ThenInclude(x => x.Project)
                    .FirstOrDefaultAsync();

                if(assignee == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "User not found.", ErrorCode = 404 });

                var assigneeProjects = assignee.User_Projects.Select(x => x.Project).ToList();

                if (assigneeProjects.Any(x => x.Id == request.ProjectId))
                    return true;

                assignee.User_Projects.Add(new Domain.Entities.User_Project
                {
                    UserId = assignee.Id,
                    ProjectId = project.Id
                });

                await _context.SaveChangesAsync();
                return true;

            }
        }
    }
}
