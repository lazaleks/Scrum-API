using AutoMapper;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Core.ApiModels
{
    public class ProjectJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketListJson> TicketLists { get; set; }

        public class Profiler : Profile
        {
            public Profiler()
            {
                CreateMap<Project, ProjectJson>();
                CreateMap<User_Project, ProjectJson>()
                    .ForMember(x => x.CreatedAt, opt => opt.MapFrom(p => p.Project.CreatedAt))
                    .ForMember(x => x.Id, opt => opt.MapFrom(p => p.Project.Id))
                    .ForMember(x => x.Name, opt => opt.MapFrom(p => p.Project.Name))
                    .ForMember(x => x.TicketLists, opt => opt.MapFrom(p => p.Project.TicketLists));
            }
        }
    }
}
