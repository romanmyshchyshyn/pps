using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Team> Teams { get; set; }

        public List<User> Users { get; set; }

        public List<CustomTask> CustomTasks { get; set; }
    }
}
