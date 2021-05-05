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
    public class EditProject
    {
        public class Command : IRequest<ProjectJson>
        {
            public int UserId { get; set; }
            public int ProjectId { get; set; }
            public string Name { get; set; }
        }
        public class Handler : IRequestHandler<Command,ProjectJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProjectJson> Handle(Command request, CancellationToken cancellationToken)
            {
                if (String.IsNullOrEmpty(request.Name) || String.IsNullOrWhiteSpace(request.Name))
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project must have a name.", ErrorCode = 400 });

                var user = await _context.Users.Where(x => x.Id == request.UserId)
                    .Include(x => x.User_Projects)
                    .ThenInclude(x => x.Project)
                    .FirstOrDefaultAsync();

                var projects = user.User_Projects.Select(x => x.Project).ToList();

                var project = projects.Where(x => x.Id == request.ProjectId).FirstOrDefault();
                if (project == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project not found.", ErrorCode = 404 });

                if (projects.Any(x => x.Name == request.Name))
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "There is already a project with that name.", ErrorCode = 400 });


                project.Name = request.Name;
                await _context.SaveChangesAsync();

                return _mapper.Map<ProjectJson>(project);
            }
        }
    }
}
