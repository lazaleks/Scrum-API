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
    public class GetProject
    {
        public class Query : IRequest<ProjectJson>
        {
            public int UserId { get; set; }
            public int ProjectId { get; set; }
        }
        public class Handler : IRequestHandler<Query,ProjectJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProjectJson> Handle(Query request, CancellationToken cancellationToken)
            {
                var project = await _context.User_Projects.Where(x => x.ProjectId == request.ProjectId && x.UserId == request.UserId)
                    .Select(x => x.Project)
                    .Include(x => x.TicketLists)
                    .FirstOrDefaultAsync();

                if (project == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project not found.", ErrorCode = 404 });

                return _mapper.Map<ProjectJson>(project);
            }
        }
    }
}
