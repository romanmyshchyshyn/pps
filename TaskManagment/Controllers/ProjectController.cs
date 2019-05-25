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
    }
}
