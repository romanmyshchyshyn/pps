using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using Services.Interfaces;
using Services.Filters;
using Services.Dto;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _service;

        public TeamController(ITeamService service)
        {
            _service = service;
        }

        public IActionResult Index(TeamFilter filter)
        {
            ViewBag.ProjectId = filter?.ProjectId;
            var list = _service.Get(filter).ToList();
            return View(list);
        }

        public IActionResult Details(string id)
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

            return View(dto);
        }

        public IActionResult Create(string projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,ProjectId")] TeamDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Add(dto);
                return RedirectToAction(nameof(Index), new { ProjectId = dto.ProjectId });
            }
            return View(dto);
        }

        public IActionResult Edit(string id)
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

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name")] TeamDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _service.Update(dto);
                return RedirectToAction(nameof(Index), new { ProjectId = dto.ProjectId });
            }

            return View(dto);
        }

        public IActionResult Delete(string id)
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

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string projectId, string id)
        {
            _service.Remove(id);
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }
    }
}
