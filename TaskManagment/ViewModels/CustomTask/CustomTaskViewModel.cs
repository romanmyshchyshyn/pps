using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.ViewModels.CustomTask
{
    public class CustomTaskViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("deadline")]
        public DateTime Deadline { get; set; }

        [JsonProperty("estimateTime")]
        public int? EstimateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("userCreatorFullName")]
        public string UserCreatorFullName { get; set; }

        [JsonProperty("userAssigneeId")]
        public string UserAssigneeId { get; set; }

        [JsonProperty("userAssigneeFullName")]
        public string UserAssigneeFullName { get; set; }

        [JsonProperty("userAssigneeImagePath")]
        public string UserAssigneeImagePath { get; set; }
    }
}
