using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string ImagePath { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
