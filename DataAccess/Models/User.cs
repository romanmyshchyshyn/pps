using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImagePath { get; set; }

        public int LeadOfTeamID { get; set; }
        public Team LeadOfTeam { get; set; }

        public int UserTeamId { get; set; }
        public Team UserTeam { get; set; }
    }
}
