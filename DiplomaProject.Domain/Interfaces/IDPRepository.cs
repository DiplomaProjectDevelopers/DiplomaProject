using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Interfaces
{
    public interface IDPRepository : IDisposable
    {
        T GetById<T>(int id) where T : class;

        DbSet<T> GetAll<T>() where T : class;

        Task<T> Insert<T>(T item) where T : class;

        Task<T> Update<T>(T item) where T : class;
        Task<IEnumerable<T>> UpdateRange<T>(IEnumerable<T> entities) where T : class;
        Task DeleteById<T>(int id) where T : class;

        Task Delete<T>(T item) where T : class;

        Task<SignInResult> SignInAsync(string username, string password, bool rememberme, bool lockoutOnFailure = false);
        Task SignOutAsync();
        Task Save();
    }
}
