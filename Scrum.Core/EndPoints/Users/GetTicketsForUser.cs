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

namespace Scrum.Core.EndPoints.Users
{
    public class GetTicketsForUser
    {
        public class Query : IRequest<List<TicketJson>>
        {
            public int UserId { get; set; }
        }
        public class Handler : IRequestHandler<Query,List<TicketJson>>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TicketJson>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tickets = await _context.Tickets.AsNoTracking()
                    .Where(x => x.AssignedTo == request.UserId)
                    .ToListAsync();

                return _mapper.Map<List<TicketJson>>(tickets);
            }
        }
    }
}
