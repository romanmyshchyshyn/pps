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
        private readonly ICustomTaskStatusService _customTaskStatusService;

        public CustomTaskController(ICustomTaskService service, ICustomTaskStatusService customTaskStatusService)
        {
            _service = service;
            _customTaskStatusService = customTaskStatusService;
        }        
    }
}
