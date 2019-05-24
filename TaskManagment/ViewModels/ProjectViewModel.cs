using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.ViewModels
{
    public class ProjectViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Project manager")]
        public string ProjectManagerUserName { get; set; }

        [Display(Name = "Owner")]
        public string OwnerUserName { get; set; }
    }
}
