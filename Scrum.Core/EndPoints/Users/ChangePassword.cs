using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.Core.Utils;
using Scrum.Core.Validations.ValidationModels;
using Scrum.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Users
{
    public class ChangePassword
    {
        public class Command : IRequest<UserJson>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
        }
        public class Handler : IRequestHandler<Command, UserJson>
        {
            private readonly ScrumContext _context;
            private readonly IMapper _mapper;

            public Handler(ScrumContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserJson> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.Username == request.Username).FirstOrDefaultAsync();

                if (user == null)
                    throw new BusinessException(new ValidationResultModel { Message = "No such User!", ErrorCode = 404 });

                if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    throw new BusinessException(new ValidationResultModel { Message = "Wrong Password!", ErrorCode = 401 });

                PasswordHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.SaveChangesAsync();

                return _mapper.Map<UserJson>(user);
            }
        }
    }
}
