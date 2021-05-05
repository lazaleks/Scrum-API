using Microsoft.AspNetCore.Mvc;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Api.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public User User;
    }
}
