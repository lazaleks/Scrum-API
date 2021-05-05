using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.DataAccess;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Projects
{
    public class CreateProject
    {
        public class Command : IRequest<ProjectJson>
        {
            public int UserId { get; set; }
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

            public async  Task<ProjectJson> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.Id == request.UserId)
                    .Include(x => x.User_Projects)
                    .ThenInclude(x => x.Project)
                    .FirstOrDefaultAsync();

                var project = new Project
                {
                    Name = request.Name,
                    CreatedAt = DateTime.Now
                };
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                user.User_Projects.Add(new User_Project
                {
                    UserId = user.Id,
                    ProjectId = project.Id
                });

                await _context.SaveChangesAsync();

                return _mapper.Map<ProjectJson>(project);
            }
        }
    }
}
