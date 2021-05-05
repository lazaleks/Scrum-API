using AutoMapper;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Core.ApiModels
{
    public class TicketListJson
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketJson> Tickets { get; set; }

        public class Profiler : Profile
        {
            public Profiler()
            {
                CreateMap<TicketList, TicketListJson>();
            }
        }
    }
}
