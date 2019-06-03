using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Services.Interfaces;
using Services.Filters;
using Services.Dto;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManagment.ViewModels;
using Services.Exceptions;
using TaskManagment.ViewModels.Project;
using TaskManagment.Roles;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Implementation;
using TaskManagment.ViewModels.CustomTaskStatus;
using TaskManagment.ViewModels.CustomTask;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _service;        
        private readonly ICustomTaskStatusService _customTaskStatusService;
        private readonly ICustomTaskService _customTaskService;

        private readonly IEmailSender _emailSender;
        private readonly string inviteTokenProvider = "Default";
        private readonly string invitePurpose = "invitePurpose";

        public ProjectController(IProjectService service,
            UserManager<User> userManager, 
            IEmailSender emailSender, 
            ICustomTaskStatusService customTaskStatusService,
            ICustomTaskService customTaskService) : base(userManager)
        {
            _service = service;            
            _emailSender = emailSender;
            _customTaskStatusService = customTaskStatusService;
            _customTaskService = customTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await GetAuthUserAsync();

            string projectId = user.ProjectId;
            if (projectId == null)
            {
                return BadRequest();
            }

            ProjectDto dto = _service.Get(projectId);
            if (dto == null)
            {
                return BadRequest();
            }

            List<CustomTaskFragmentViewModel> customTaskFragmentViewModelList = dto.CustomTasks.Select(ctDto =>
                new CustomTaskFragmentViewModel { Id = ctDto.Id, Name = ctDto.Name, Status = ctDto.Status, UserAssigneeImagePath = ctDto.UserAssignee.ImagePath }).ToList();

            bool isOwner = await _userManager.IsInRoleAsync(user, Role.Owner);

            CustomTaskStatusFilter customTaskStatusFilter = new CustomTaskStatusFilter();
            List<CustomTaskStatusDto> customTaskStatusDtoList = _customTaskStatusService.Get(customTaskStatusFilter).ToList();
            List<CustomTaskStatusViewModel> customTaskStatusViewModelList = customTaskStatusDtoList
                .Select(ctsDto => new CustomTaskStatusViewModel { Name = ctsDto.Name, Index = ctsDto.Index })
                .OrderBy(ctsVm => ctsVm.Index)
                .ToList();

           ProjectViewModel vm = 
                new ProjectViewModel { Id = dto.Id, Name = dto.Name, isCanAddMember = isOwner, CustomTaskStatuses = customTaskStatusViewModelList, CustomTasks = customTaskFragmentViewModelList };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var projectId = Guid.NewGuid().ToString();
                ProjectDto dto = new ProjectDto { Id = projectId, Name = vm.Name };
                _service.Add(dto);

                User user = await GetAuthUserAsync();
                user.ProjectId = projectId;
                await _userManager.AddToRoleAsync(user, Role.Owner);
                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InviteMember(string email, string projectId, string projectName)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("There is no user with such email.");
            }

            if (user.ProjectId != null)
            {
                return BadRequest("User already has a project.");
            }

            User authUser = await GetAuthUserAsync();
            bool isOwner = await _userManager.IsInRoleAsync(authUser, Role.Owner);
            if (!isOwner)
            {
                return Forbid();
            }

            string token = await _userManager.GenerateUserTokenAsync(user, inviteTokenProvider, invitePurpose);

            var callbackUrl = Url.Action(
                       "AcceptInviteMember",
                       "Project",
                       new { userId = user.Id, token = token, projectId = projectId },
                       protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Invitation",  $"Accept invitation for project '{projectName}': <a href='{callbackUrl}'>link</a>");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AcceptInviteMember(string userId, string token, string projectId)
        {
            if (userId == null || token == null || projectId == null)
            {
                ViewData["ErrorSubtitle"] = "This link is not valid.";
                return View("Error");
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            User authUser = await GetAuthUserAsync();            
            if (authUser.Id != userId)
            {
                ViewData["ErrorSubtitle"] = "Check if you signed in with correct account for this link.";
                return View("Error");
            }

            bool isTokenValid = await _userManager.VerifyUserTokenAsync(authUser, inviteTokenProvider, invitePurpose, token);
            if (!isTokenValid)
            {
                ViewData["ErrorSubtitle"] = "This link is not valid anymore.";
                return View("Error");
            }

            ProjectDto projectDto = _service.Get(projectId);
            if (projectDto == null)
            {
                ViewData["ErrorSubtitle"] = "This link is not valid anymore.";
                return View("Error");
            }

            if (user.ProjectId != null)
            {
                ViewData["ErrorSubtitle"] = "User already has a project.";
                return View("Error");
            }

            user.ProjectId = projectId;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
