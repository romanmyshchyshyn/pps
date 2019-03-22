using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string ImagePath { get; set; }

        public int TeamLeadId { get; set; }
        public User Teamlead { get; set; }

        public int ProjectId { get; set; }
        public Project RelatedProject { get; set; }

        public List<User> Users { get; set; }
    }
}
