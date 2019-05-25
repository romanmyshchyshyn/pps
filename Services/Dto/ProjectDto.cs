using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class ProjectDto
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }        
    }
}
