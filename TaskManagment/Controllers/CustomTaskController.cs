using DataAccess.Models;
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
    public class CustomTaskController : Controller
    {
        private readonly ICustomTaskService _service;
        private readonly ICustomTaskStatusService _customTaskStatusService;
        private readonly UserManager<User> _userManager;

        public CustomTaskController(ICustomTaskService service, ICustomTaskStatusService customTaskStatusService, UserManager<User> userManager)
        {
            _service = service;
            _customTaskStatusService = customTaskStatusService;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Create(CreateCustomTaskViewModel cVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userId);
            CustomTaskDto dto = new CustomTaskDto { Name = cVm.Name,  UserAssigneeId = userId, UserCreatorId = userId, Status = cVm.Status, ProjectId = cVm.ProjectId };
            _service.Add(dto);

            CustomTaskViewModel vm = new CustomTaskViewModel
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
