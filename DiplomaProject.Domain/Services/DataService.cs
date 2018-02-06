using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Services
{
    public class DataService : IDPService
    {
        private IDPRepository repository;
        public DataService(IDPRepository repository)
        {
            this.repository = repository;
        }
        public async Task Delete<T>(T item) where T : class
        {
            await repository.Delete<T>(item);
        }

        public async Task DeleteById<T>(int id) where T : class
        {
            await repository.DeleteById<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            return await repository.GetAll<T>();
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
            return await repository.GetById<T>(id);
        }

        public async Task Insert<T>(T item) where T : class
        {
             await repository.Insert<T>(item);
        }

        public Task Update<T>(T item) where T : class
        {
            return repository.Update<T>(item);
        }

        public async Task<SignInResult> SignInAsync(LoginViewModel loginModel, bool lockoutInFailure = false)
        {

            return await repository.SignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, lockoutInFailure);
        }

        public void Save()
        {
            repository.Save();
        }
    }
}
