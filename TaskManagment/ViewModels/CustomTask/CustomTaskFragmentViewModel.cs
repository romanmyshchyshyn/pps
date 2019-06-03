using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.ViewModels.CustomTask
{
    public class CustomTaskFragmentViewModel
    {
        [Display(Name = "Id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        [JsonProperty("name")]
        public string Name { get; set; }        

        [JsonProperty("status")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [JsonProperty("userAssigneeImagePath")]
        public string UserAssigneeImagePath { get; set; }
    }
}
