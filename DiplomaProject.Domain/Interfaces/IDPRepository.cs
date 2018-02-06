using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Interfaces
{
    public interface IDPRepository
    {
        Task<T> GetById<T>(int id) where T: class;

        Task<IEnumerable<T>> GetAll<T>() where T : class;

        Task<T> Insert<T>(T item) where T : class;

        Task<T> Update<T>(T item) where T : class;

        Task DeleteById<T>(int id) where T : class;

        Task Delete<T>(T item) where T : class;

        Task<SignInResult> SignInAsync(string username, string password, bool rememberme, bool lockoutOnFailure = false);
        void Save();
    }
}
