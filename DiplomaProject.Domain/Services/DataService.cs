using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DiplomaProject.Domain.Services
{
    public class DataService : IDPService
    {
        private IDPRepository repository;
        private SignInManager<Entities.User> signInManager;
        private UserManager<Entities.User> userManager;
        public DataService(IDPRepository repository, UserManager<Entities.User> userManager, SignInManager<Entities.User> signInManager)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task Delete<T>(T item) where T : class
        {
            await repository.Delete<T>(item);
        }

        public async Task DeleteById<T>(int id) where T : class
        {
            await repository.DeleteById<T>(id);
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return repository.GetAll<T>();
        }

        public T GetById<T>(int id) where T : class
        {
            return repository.GetById<T>(id);
        }

        public async Task<T> Insert<T>(T item) where T : class
        {
            return await repository.Insert(item);
        }

        public async Task<T> Update<T>(T item) where T : class
        {
            return await repository.Update(item);
        }

        public async Task<IEnumerable<T>> UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            return await repository.UpdateRange(entities);
        }

        public async Task<SignInResult> SignInAsync(LoginViewModel loginModel, bool lockoutInFailure = false)
        {

            return await repository.SignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, lockoutInFailure);
        }

        public async Task SignOutAsync()
        {
            await repository.SignOutAsync();
        }
        public void Save()
        {
            repository.Save();
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }
        public void Dispose()
        {
            repository.Dispose();
        }
    }
}
