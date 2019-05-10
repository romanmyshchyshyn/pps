using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dto
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }        

        public string OwnerId { get; set; }        

        public string ProjectManagerId { get; set; }     
    }
}
