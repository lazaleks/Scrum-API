using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
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
    public class DeleteProject
    {
        public class Command : IRequest<bool>
        {
            public int ProjectId { get; set; }
            public int UserId { get; set; }
        }
        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly ScrumContext _context;

            public Handler(ScrumContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var userProjects = await _context.User_Projects.Where(x => x.UserId == request.UserId).Include(x => x.Project).ToListAsync();
                var userProject = userProjects.Where(x => x.ProjectId == request.ProjectId && x.UserId == request.UserId).FirstOrDefault();

                if (userProject == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project not found.", ErrorCode = 404 });

                _context.User_Projects.Remove(userProject);
                await _context.SaveChangesAsync();

                var project = await _context.Projects.Where(x => x.Id == request.ProjectId)
                    .Include(x => x.User_Projects)
                    .FirstOrDefaultAsync();

                if(project.User_Projects.Count() == 0)
                {
                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync();
                }    

                return true;
            }
        }
    }
}
