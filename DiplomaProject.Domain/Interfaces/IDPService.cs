using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Interfaces
{
    public interface IDPService
    {
        T GetById<T>(int id) where T : class;

        DbSet<T> GetAll<T>() where T : class;

        Task<T> Insert<T>(T item) where T : class;

        Task<T> Update<T>(T item) where T : class;

        Task DeleteById<T>(int id) where T : class;

        Task Delete<T>(T item) where T : class;

        Task<SignInResult> SignInAsync(LoginViewModel model, bool lockoutOnFailure = false);

        Task SignOutAsync();

        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<IdentityResult> AddUserAsync(User user, string password);
        void Save();
    }
}
