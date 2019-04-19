using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using Services.Implementation;
using Services.Dto;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Details(string id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Create()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OwnerId,ProjectManagerId")] ProjectDto project)
        {
            if (ModelState.IsValid)
            {
                _service.Add(project);
                return RedirectToAction(nameof(Index));
            }

            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
            //ViewData["ProjectManagerId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectManagerId);
            //return View(project);

            throw new NotImplementedException();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = _service.Get(id);
            if (project == null)
            {
                return NotFound();
            }
            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
            //ViewData["ProjectManagerId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectManagerId);
            //return View(project);

            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,OwnerId,ProjectManagerId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(project);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ProjectExists(project.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
            //ViewData["ProjectManagerId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectManagerId);
            //return View(project);

            throw new NotImplementedException();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var project = _context.Projects
            //    .Include(p => p.Owner)
            //    .Include(p => p.ProjectManager)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (project == null)
            //{
            //    return NotFound();
            //}

            //return View(project);

            throw new NotImplementedException();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
             _service.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
