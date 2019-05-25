using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string ImageId { get; set; }
        public Image Image { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public List<CustomTask> CustomTasks { get; set; }

        public List<User> Users { get; set; }
    }
}
