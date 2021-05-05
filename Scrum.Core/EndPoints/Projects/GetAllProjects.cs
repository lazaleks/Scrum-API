using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Projects
{
    public class GetAllProjects
    {
        public class Query : IRequest<List<ProjectJson>>
        {
            public int UserId { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<ProjectJson>>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ProjectJson>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projects = await _context.User_Projects
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .Select(x => x.Project)
                    .ToListAsync();

                return _mapper.Map<List<ProjectJson>>(projects);
            }
        }
    }
}
