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

namespace TaskManagment.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender _emailSender;
        private readonly string inviteTokenProvider = "Default";
        private readonly string invitePurpose = "invitePurpose";

        public ProjectController(IProjectService service, UserManager<User> userManager, IEmailSender emailSender)
        {
            _service = service;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userId);
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

            bool isOwner = await _userManager.IsInRoleAsync(user, Role.Owner);

            ProjectViewModel vm = new ProjectViewModel { Id = dto.Id, Name = dto.Name, isCanAddMember = isOwner };

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

                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                User user = await _userManager.FindByIdAsync(userId);
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

            string authUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User authUser = await _userManager.FindByIdAsync(authUserId);
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
                return BadRequest();
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            bool isValidToken = await _userManager.VerifyUserTokenAsync(user, inviteTokenProvider, invitePurpose, token);
            if (!isValidToken)
            {
                return BadRequest();
            }

            ProjectDto projectDto = _service.Get(projectId);
            if (projectDto == null)
            {
                return BadRequest();
            }

            user.ProjectId = projectId;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }


    }
}
