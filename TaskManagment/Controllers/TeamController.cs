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
    }
}
