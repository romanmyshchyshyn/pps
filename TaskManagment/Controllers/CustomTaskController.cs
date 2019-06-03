﻿using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.ViewModels.CustomTask;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class CustomTaskController : BaseController
    {
        private readonly ICustomTaskService _service;
        private readonly ICustomTaskStatusService _customTaskStatusService;

        public CustomTaskController(ICustomTaskService service, ICustomTaskStatusService customTaskStatusService, UserManager<User> userManager)
             : base(userManager)
        {
            _service = service;
            _customTaskStatusService = customTaskStatusService;
        }
        
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Create(CreateCustomTaskViewModel cVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await GetAuthUserAsync();
            CustomTaskDto dto = new CustomTaskDto
            {
                Name = cVm.Name,
                Status = cVm.Status,
                ProjectId = cVm.ProjectId,
                UserAssigneeId = user.Id,
                UserCreatorId = user.Id
            };

            _service.Add(dto);

            CustomTaskFragmentViewModel vm = new CustomTaskFragmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                UserAssigneeImagePath = user.ImagePath
            };

            return Ok(vm);
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult UpdateStatus(string id, string status)
        {
            if (id == null || status == null)
            {
                return BadRequest();
            }

            ((ICustomTaskService)_service).UpdateStatus(id, status);

            return Ok();
        }        
    }
}
