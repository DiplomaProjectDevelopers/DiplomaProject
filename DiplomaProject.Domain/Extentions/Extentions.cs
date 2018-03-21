using DiplomaProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Extentions
{
    public static class Extentions
    {
        public static async Task<string> GetRoleAsync(this UserManager<User> userManager, User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count <= 0)
                return null;
            var role = roles[0];
            return role;
        }
    }
}
