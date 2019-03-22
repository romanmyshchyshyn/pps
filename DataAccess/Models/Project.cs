using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }

        public List<Team> Teams { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public int ProjectManagerId { get; set; }
        public User ProjectManager { get; set; }
    }
}
