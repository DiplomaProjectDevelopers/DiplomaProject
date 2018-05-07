using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Entities
{
    public class DPClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {

        public DPClaimsPrincipalFactory(
            UserManager<User> userManager
            , RoleManager<Role> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        protected override Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {

            return base.GenerateClaimsAsync(user);
        }
        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.GivenName, user.FirstName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.Surname, user.LastName),
                });
            }

            return principal;
        }
    }
}
