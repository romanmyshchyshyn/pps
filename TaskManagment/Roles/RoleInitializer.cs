using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.Roles
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(Role.Owner) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Owner));
            }
            if (await roleManager.FindByNameAsync(Role.ProjectManager) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.ProjectManager));
            }            
        }
    }
}

