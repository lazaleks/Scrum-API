using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Domain.Entities
{
    public class User
    {
        public User()
        {
            User_Projects = new List<User_Project>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<User_Project> User_Projects { get; set; }
    }
}
