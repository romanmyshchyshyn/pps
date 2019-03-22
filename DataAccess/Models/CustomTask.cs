using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class CustomTask
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeadLine { get; set; }
        public string Status { get; set; }

        public int UserCreatorId { get; set; }
        public User UserCreator { get; set; }

        public int UserAssigneeId { get; set; }
        public User UserAssignee { get; set; }

        public List<CustomTask> RelatedTasks { get; set; }

        public List<string> ImagesPathes { get; set; }
    }
}
