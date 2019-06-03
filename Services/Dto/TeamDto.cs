using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class TeamDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public List<CustomTask> CustomTasks { get; set; }

        public List<User> Users { get; set; }
    }
}
