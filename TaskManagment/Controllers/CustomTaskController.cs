using Microsoft.AspNetCore.Authorization;
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

namespace TaskManagment.Controllers
{
    [Authorize]
    public class CustomTaskController : Controller
    {
        private readonly ICustomTaskService _service;

        public CustomTaskController(ICustomTaskService service)
        {
            _service = service;
        }

        public IActionResult Index(CustomTaskFilter filter)
        {
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,CreationDate,DeadLine,Status")] CustomTaskDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Add(dto);
                return RedirectToAction(nameof(Index));
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
        public IActionResult Edit(string id, [Bind("Id,Name,Description,CreationDate,DeadLine,Status")] CustomTaskDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _service.Update(dto);
                return RedirectToAction(nameof(Index));
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
        public IActionResult DeleteConfirmed(string id)
        {
            _service.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
