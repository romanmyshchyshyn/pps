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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] ProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ProjectDto dto = new ProjectDto { Name = vm.Name };
                _service.Add(dto);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
