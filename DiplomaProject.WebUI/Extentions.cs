using DiplomaProject.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.WebUI
{
    public static class Extentions
    {
        //public static IWebHost Seed(this IWebHost webhost)
        //{
        //    using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        // alternatively resolve UserManager instead and pass that if only think you want to seed are the users
        //        using (var dbContext = scope.ServiceProvider.GetRequiredService<DiplomaProjectContext>())
        //        {
        //            SeedData.SeedAsync(dbContext).GetAwaiter().GetResult();
        //        }
        //    }
        //}

        //public static class SeedData
        //{
        //    public static async Task SeedAsync(DiplomaProjectContext dbContext)
        //    {
        //        dbContext.Users.Add(new User { Id = 1, u = "admin", PasswordHash = ... });
        //    }
        //}
    }
}
