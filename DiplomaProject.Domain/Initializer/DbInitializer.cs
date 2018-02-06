using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiplomaProject.Domain.Initializer
{
    public class DbInitializer : IDbInitializer
    {


        private readonly DiplomaProjectContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;

        public DbInitializer(
            DiplomaProjectContext context,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //This example just creates an Administrator role and one Admin users
        public async void Initialize()
        {
            try
            {
                //create database schema if none exists
                _context.Database.Migrate();

                //If there is already an Administrator role, abort
                if (!_context.Roles.Any(r => r.Name == "Administrator"))
                {
                    _roleManager.CreateAsync(new UserRole("Administrator")).GetAwaiter().GetResult();
                }


                //Create the Administartor Role

                //Create the default Admin account and apply the Administrator role
                string email = "hakobpapazyan2@gmail.com";
                string userName = "admin9";
                string password = "Admin0chka!";
                if (!_context.Users.Any(u => u.Email == email))
                {
                    _userManager.CreateAsync(new User { UserName = userName, Email = email, EmailConfirmed = true }, password).GetAwaiter().GetResult();
                }
                if (_context.Users.Any(u => u.Email == email))
                {
                    await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(userName), "Administrator");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
