using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dto
{
    public class TeamDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string TeamLeadId { get; set; }

        public string ProjectId { get; set; }
    }
}
