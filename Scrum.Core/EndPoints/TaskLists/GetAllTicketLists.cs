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

namespace Scrum.Core.EndPoints.TaskLists
{
    public class GetAllTicketLists
    {
        public class Query : IRequest<List<TicketListJson>>
        {
            public int ProjectId { get; set; }
        }
        public class Handler : IRequestHandler<Query,List<TicketListJson>>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TicketListJson>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ticketLists = await _context.TicketLists.AsNoTracking()
                    .Where(x => x.ProjectId == request.ProjectId)
                    .ToListAsync();

                return _mapper.Map<List<TicketListJson>>(ticketLists);
            }
        }
    }
}
