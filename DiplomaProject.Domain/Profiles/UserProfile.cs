using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ProfessionAdminViewModel>();
            CreateMap<User, RegisterViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}
