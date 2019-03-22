using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImagePath { get; set; }

        public int LeadOfTeamID { get; set; }
        public Team LeadOfTeam { get; set; }

        public int UserTeamId { get; set; }
        public Team UserTeam { get; set; }

        public int IsOwnerOfProjectId { get; set; }
        public Project IsOwnerOfProject { get; set; }

        public int IsManagerId { get; set; }
        public Project IsManager { get; set; }

        public int CreatorOfTaskID { get; set; }
        public CustomTask CreatorOftask { get; set; }

        public int AssigneeOfTaskId { get; set; }
        public CustomTask AssigneeOfTask { get; set; }


    }
}
