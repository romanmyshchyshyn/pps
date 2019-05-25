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

        public List<User> Users { get; set; }
    }
}
