﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class CustomTaskDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Deadline { get; set; }
        public int? EstimateTime { get; set; }
        public string Status { get; set; }

        public string UserCreatorId { get; set; }
        public User UserCreator { get; set; }

        public string UserAssigneeId { get; set; }
        public User UserAssignee { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string TeamId { get; set; }
        public Team Team { get; set; }

        public List<CustomFile> CustomFiles { get; set; }
    }
}
