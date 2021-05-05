using AutoMapper;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrum.Core.ApiModels
{
    public class TicketJson
    {
        public int Id { get; set; }
        public int TicketListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public bool Done { get; set; }

        public class Profiler : Profile
        {
            public Profiler()
            {
                CreateMap<Ticket, TicketJson>();
            }
        }
    }
}
