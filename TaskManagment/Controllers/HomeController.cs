using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagment.CustomClaims;
using TaskManagment.Models;

namespace TaskManagment.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<User> userManager)
            : base(userManager)
        {

        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetAuthUserAsync();
                var projectId = user.ProjectId;
                if (projectId != null)
                {
                    return RedirectToAction("Index", "Project");
                }
            }

            return View();
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
