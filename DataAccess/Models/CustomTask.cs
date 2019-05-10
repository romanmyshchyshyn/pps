using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class CustomTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }

        public string UserCreatorId { get; set; }
        public User UserCreator { get; set; }

        public string UserAssigneeId { get; set; }
        public User UserAssignee { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public List<Image> Images { get; set; }
    }
}
