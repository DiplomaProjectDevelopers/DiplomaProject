using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Repositories
{
    public class DPRepository : IDPRepository
    {
        private readonly DiplomaProjectContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        //private DbSet<T> entities;
        public DPRepository(DiplomaProjectContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns></returns>      
        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            return await Task.FromResult(context.Set<T>().ToList());
        }

        /// <summary>
        /// Get element by id.
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        public async Task<T> GetById<T>(int id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Insert new item.
        /// </summary>
        /// <param name = "item" ></ param >
        /// < returns ></ returns >
        public async Task<T> Insert<T>(T item) where T : class
        {
            await context.Set<T>().AddAsync(item);
            await Task.Run(() => Save());
            return item;
        }

        /// <summary>
        /// Delete item by id.
        /// </summary>
        /// <param name = "id" ></ param >
        public async Task DeleteById<T>(int id) where T : class
        {
            T entityToDelete = await GetById<T>(id);
            if (entityToDelete == null) return;
             await Task.Run(() => Delete(entityToDelete));
        }

        /// <summary>
        /// Delete item.
        /// </summary>
        /// <param name = "entityToDelete" ></ param >
        public async Task Delete<T>(T entityToDelete) where T : class
        {
            EntityEntry dbEntityEntry = context.Entry(entityToDelete);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                context.Set<T>().Attach(entityToDelete);
                context.Set<T>().Remove(entityToDelete);
            }
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                context.Set<T>().Attach(entityToDelete);
            }
            context.Set<T>().Remove(entityToDelete);
            await Task.Run(() => Save());
        }


        /// <summary>
        /// Update element.
        /// </summary>
        /// <param name = "obj" ></ param >
        /// < returns ></ returns >
        public async Task<T> Update<T>(T item) where T : class
        {
            context.Set<T>().Attach(item);
            context.Entry(item).State = EntityState.Modified;
            await Task.Run(() => Save());
            return item;
        }

        public async Task<SignInResult> SignInAsync(string username, string Password,bool RememberMe, bool lockoutInFailure = false)
        {
            return await signInManager.PasswordSignInAsync(username, Password, RememberMe, lockoutInFailure);
        }

        public async Task SignOutAsync()
        {
             await signInManager.SignOutAsync();
        }
        /// <summary>
        ///Save changes in database.
        /// </summary>
        public async void Save()
        {
            try
            {
                await context.SaveChangesAsync();
            }

            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                }
            }
        }
    }
}
