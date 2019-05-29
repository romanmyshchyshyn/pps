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

namespace TaskManagment.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        private readonly UserManager<User> _userManager;

        public ProjectController(IProjectService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            ProjectDto dto = _service.Get(id);
            ProjectViewModel vm = new ProjectViewModel { Id = dto.Id, Name = dto.Name };
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userId);

            //try
            //{
            //    await _userManager.AddToRoleAsync(user, Role.Owner);
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] ProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var projectId = Guid.NewGuid().ToString();
                ProjectDto dto = new ProjectDto { Id = projectId, Name = vm.Name };
                _service.Add(dto);
                return RedirectToAction(nameof(Index), new { id = projectId });
            }

            return RedirectToAction("Index", "Project", new { area = "" });
        }

        //public IActionResult inviteUser([Bind("Id,Name")] ProjectViewModel vm)
        //{
        //    //todo
        //}


    }
}
