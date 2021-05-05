using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Domain.Entities
{
    public class Project
    {
        public Project()
        {
            TicketLists = new List<TicketList>();
            User_Projects = new List<User_Project>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketList> TicketLists { get; set; }
        public List<User_Project> User_Projects { get; set; }
    }
}
