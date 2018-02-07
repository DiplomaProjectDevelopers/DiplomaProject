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
                string password = "Admin0chka!";
                var user = new User
                {
                    FirstName = "Hakob",
                    LastName = "Papazyan",
                    PhoneNumber = "093697343",
                    UserName = "admin9",//userName,
                    Email = "hakobpapazyan2@gmail.com",//email,
                    EmailConfirmed = true
                };
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
                }
                else
                {
                    var existUser = await _userManager.FindByEmailAsync(user.Email);
                    existUser.FirstName = user.FirstName;
                    existUser.LastName = user.LastName;
                    existUser.PhoneNumber = user.PhoneNumber;
                    await _userManager.UpdateAsync(existUser);
                    await _userManager.AddToRoleAsync(existUser, "Administrator");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
