using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.Core.ApiModels
{
    public class User_ProjectsJson
    {
        public int UserId { get; set; }
        public UserJson User { get; set; }
        public int ProjectId { get; set; }
        public ProjectJson Project { get; set; }
    }
}
