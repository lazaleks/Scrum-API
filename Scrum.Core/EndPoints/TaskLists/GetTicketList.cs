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

namespace Scrum.Core.EndPoints.TaskLists
{
    public class GetTicketList
    {
        public class Query : IRequest<TicketListJson>
        {
            public int ProjectId { get; set; }
            public int TicketListId { get; set; }
        }
        public class Handler : IRequestHandler<Query,TicketListJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TicketListJson> Handle(Query request, CancellationToken cancellationToken)
            {
                var ticketList = await _context.TicketLists.AsNoTracking()
                    .Where(x => x.Id == request.TicketListId && x.ProjectId == request.ProjectId)
                    .FirstOrDefaultAsync();

                if (ticketList == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket list not found.", ErrorCode = 404 });

                return _mapper.Map<TicketListJson>(ticketList);
            }
        }
    }
}
