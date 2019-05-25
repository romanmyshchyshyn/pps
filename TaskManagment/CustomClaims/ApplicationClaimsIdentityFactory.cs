using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.CustomClaims;

namespace TaskManagment.NewFolder
{
    public class ApplicationClaimsIdentityFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly UserManager<User> _userManager;

        public ApplicationClaimsIdentityFactory(UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        { }
        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                  new Claim(CustomClaimTypes.FullName, user.FullName),
                  new Claim(CustomClaimTypes.ImagePath, user.ImagePath)
            });

            return principal;
        }
    }
}
