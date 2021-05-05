using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Domain.Entities
{
    public class Ticket
    {
        public Ticket()
        {

        }
        public int Id { get; set; }
        public int TaskListId { get; set; }
        public TicketList TicketList { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool Done { get; set; }
    }
}
