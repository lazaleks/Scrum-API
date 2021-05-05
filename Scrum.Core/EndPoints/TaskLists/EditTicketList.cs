﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.TaskLists
{
    public class EditTicketList
    {
        public class Command : IRequest<TicketListJson>
        {
            public int TicketListId { get; set; }
            public int ProjectId { get; set; }
            [Required]
            [MaxLength(20)]
            public string Name { get; set; }
        }
        public class Handler : IRequestHandler<Command, TicketListJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TicketListJson> Handle(Command request, CancellationToken cancellationToken)
            {

                var project = await _context.Projects.Where(x => x.Id == request.ProjectId)
                    .Include(x => x.TicketLists)
                    .FirstOrDefaultAsync();

                if (project == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Project not found.", ErrorCode = 404 });

                var ticketList = project.TicketLists.Where(x => x.Id == request.TicketListId).FirstOrDefault();
                if (ticketList == null)
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "Ticket list not found.", ErrorCode = 404 });

                if (project.TicketLists.Any(x => x.Name == request.Name))
                    throw new BusinessException(new Validations.ValidationModels.ValidationResultModel { Message = "There is already a ticket list with that name.", ErrorCode = 400 });

                ticketList.Name = request.Name;

                await _context.SaveChangesAsync();

                return _mapper.Map<TicketListJson>(ticketList);
            }
        }
    }
}
