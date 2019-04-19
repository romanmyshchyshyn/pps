using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dto
{
    public class CustomTaskDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeadLine { get; set; }
        public string Status { get; set; }

        public string UserCreatorId { get; set; }       

        public string UserAssigneeId { get; set; }        
    }
}
