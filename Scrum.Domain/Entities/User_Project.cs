using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Domain.Entities
{
    public class User_Project
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
