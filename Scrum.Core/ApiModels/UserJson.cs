using AutoMapper;
using Scrum.Domain.Entities;
using System.Collections.Generic;

namespace Scrum.Core.ApiModels
{
    public class UserJson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<ProjectJson> Projects { get; set; }

        public class Profiler : Profile
        {
            public Profiler()
            {
                CreateMap<User, UserJson>()
                    .ForMember(x => x.Projects, opt => opt.MapFrom(o => o.User_Projects));
            }
        }
    }
}
