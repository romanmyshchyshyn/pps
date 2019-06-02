using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.ViewModels.CustomTask
{
    public class CreateCustomTaskViewModel
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("status")]
        public string Status { get; set; }

        [Required]
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }
    }
}
