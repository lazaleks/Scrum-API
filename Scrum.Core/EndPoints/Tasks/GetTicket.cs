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

namespace Scrum.Core.EndPoints.Tasks
{
    public class GetTicket
    {
        public class Query : IRequest<TicketJson>
        {
            public int TicketId { get; set; }
        }
        public class Handler : IRequestHandler<Query,TicketJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TicketJson> Handle(Query request, CancellationToken cancellationToken)
            {
                var ticket = await _context.Tickets.AsNoTracking()
                    .Where(x => x.Id == request.TicketId)
                    .FirstOrDefaultAsync();

                if (ticket == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket not found.", ErrorCode = 404 });

                return _mapper.Map<TicketJson>(ticket);
            }
        }
    }
}
