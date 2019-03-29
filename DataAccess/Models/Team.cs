using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public int TeamLeadId { get; set; }
        public User Teamlead { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public List<User> Users { get; set; }
    }
}
