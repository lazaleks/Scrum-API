using AutoMapper;
using MediatR;
using Scrum.Core.ApiModels;
using Scrum.Core.Exceptions;
using Scrum.Core.Utils;
using Scrum.Core.Validations.ValidationModels;
using Scrum.DataAccess;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrum.Core.EndPoints.Users
{
    public class CreateUser
    {
        public class Command : IRequest<UserJson>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
        public class Handler : IRequestHandler<Command,UserJson>
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
                if (_context.Users.Any(x => x.Username == request.Username))
                {
                    throw new BusinessException(new ValidationResultModel { Message = "Username is already taken.", ErrorCode = 400 });
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                    throw new BusinessException(new ValidationResultModel { Message = "Password is required", ErrorCode = 400 });

                PasswordHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

                var user = new User();
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Username = request.Username;
                user.Email = request.Email;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return _mapper.Map<UserJson>(user);
            }
        }
    }
}
