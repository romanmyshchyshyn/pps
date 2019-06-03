using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManagment.ViewModels.CustomTask;
using TaskManagment.ViewModels.CustomTaskStatus;

namespace TaskManagment.ViewModels.Project
{
    public class ProjectViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public bool isCanAddMember { get; set; }

        public List<CustomTaskStatusViewModel> CustomTaskStatuses { get; set; }

        public List<CustomTaskFragmentViewModel> CustomTasks { get; set; }
    }
}
