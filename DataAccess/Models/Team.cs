using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Team
    {
        public string Id { get; set; }
        public Image Image { get; set; }

        public string TeamLeadId { get; set; }
        public User Teamlead { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public List<User> Users { get; set; }
    }
}
