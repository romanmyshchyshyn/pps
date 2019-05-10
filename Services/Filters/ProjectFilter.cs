using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Filters
{
    public class ProjectFilter : BaseFilter
    {
        public string OwnerId { get; set; }
        public string ProjectManagerId { get; set; }
    }
}
