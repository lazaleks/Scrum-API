using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Users
{
    public class GetAllUsers
    {
        public class Query : IRequest<List<UserJson>>
        {

        }
        public class Handler : IRequestHandler<Query,List<UserJson>>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UserJson>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync();

                return _mapper.Map<List<UserJson>>(users);
            }
        }
    }
}
