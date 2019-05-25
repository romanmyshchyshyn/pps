using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class CustomFile
    {
        public string Id { get; set; }
        public string Path { get; set; }

        public string CustomTaskId { get; set; }
        public CustomTask CustomTask { get; set; }
    }
}
