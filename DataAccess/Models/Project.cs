using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Team> Teams { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public string ProjectManagerId { get; set; }
        public User ProjectManager { get; set; }
    }
}
