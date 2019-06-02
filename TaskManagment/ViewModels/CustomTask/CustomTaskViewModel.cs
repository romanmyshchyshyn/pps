using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.ViewModels.CustomTask
{
    public class CustomTaskViewModel
    {
        [Display(Name = "Id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [JsonProperty("creationDate")]
        [Display(Name = "Creation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("deadline")]
        [Display(Name = "Deadline")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Deadline { get; set; }

        [JsonProperty("estimateTime")]
        [Display(Name = "Estimate Time")]
        public int? EstimateTime { get; set; }

        [JsonProperty("status")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [JsonProperty("userAssigneeImagePath")]
        public string UserAssigneeImagePath { get; set; }
    }
}
