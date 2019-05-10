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

        public async Task<IActionResult> Index(ProjectFilter filter)
        {
            string ownerId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (filter == null)
            {
                filter = new ProjectFilter { OwnerId = ownerId };
            }
            else
            {
                filter.OwnerId = ownerId;
            }

            var list = _service.Get(filter).ToList();
            var listViewModel = new List<ProjectViewModel>();
            foreach (var item in list)
            {
                listViewModel.Add(await MapToViewModel(item));
            }

            return View(listViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dto = _service.Get(id);
            if (dto == null)
            {
                return NotFound();
            }

            ProjectViewModel vm = await MapToViewModel(dto);

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProjectManagerUserName")] ProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vm.OwnerUserName = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name).Value;
                    ProjectDto dto = await MapToDto(vm);
                    _service.Add(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (ObjectNotFoundException)
                {
                    ModelState.AddModelError("ProjectManagerUserName", "User does not exist. Invalid login");
                    vm.ProjectManagerUserName = null;
                    return View(vm);
                }                
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dto = _service.Get(id);
            if (dto == null)
            {
                return NotFound();
            }

            ProjectViewModel vm = await MapToViewModel(dto);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ProjectManagerUserName,OwnerUserName")] ProjectViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ProjectDto dto = await MapToDto(vm);
                    _service.Update(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (ObjectNotFoundException)
                {
                    return View(vm);
                }                
            }

            return View(vm);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dto = _service.Get(id);
            if (dto == null)
            {
                return NotFound();
            }

            ProjectViewModel vm = await MapToViewModel(dto);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _service.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProjectDto> MapToDto(ProjectViewModel vm)
        {
            User projectManager = await _userManager.FindByNameAsync(vm.ProjectManagerUserName);
            User owner = await _userManager.FindByNameAsync(vm.OwnerUserName);
            bool isObjectNotFound = false;
            if (projectManager == null)
            {
                AddUserNotExistModelError("ProjectManagerUserName");
                vm.ProjectManagerUserName = null;
                isObjectNotFound = true;
            }

            if (owner == null)
            {
                AddUserNotExistModelError("OwnerUserName");
                vm.OwnerUserName = null;
                isObjectNotFound = true;
            }

            if (isObjectNotFound)
            {
                throw new ObjectNotFoundException();
            }

            ProjectDto dto = new ProjectDto
            {
                Id = vm.Id,
                Name = vm.Name,
                OwnerId = owner.Id,
                ProjectManagerId = projectManager.Id
            };

            return dto;
        }

        private async Task<ProjectViewModel> MapToViewModel(ProjectDto dto)
        {
            User projectManager = await _userManager.FindByIdAsync(dto.ProjectManagerId);
            User owner = await _userManager.FindByIdAsync(dto.OwnerId);
            bool isObjectNotFound = false;
            if (projectManager == null)
            {
                AddUserNotExistModelError("ProjectManagerUserName");
                dto.ProjectManagerId = null;
                isObjectNotFound = true;
            }

            if (owner == null)
            {
                AddUserNotExistModelError("OwnerUserName");
                dto.OwnerId = null;
                isObjectNotFound = true;
            }

            if (isObjectNotFound)
            {
                throw new ObjectNotFoundException();
            }

            ProjectViewModel vm = new ProjectViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                OwnerUserName = owner.UserName,
                ProjectManagerUserName = projectManager.UserName
            };

            return vm;
        }

        private void AddUserNotExistModelError(string fieldName)
        {
            ModelState.AddModelError(fieldName, "User does not exist. Invalid login");
        }
    }
}
