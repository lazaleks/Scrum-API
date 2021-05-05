using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Domain.Entities
{
    public class TicketList
    {
        public TicketList()
        {
            Tickets = new List<Ticket>();
        }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
