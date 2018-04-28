using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Profiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<SubjectModule, SubjectModuleViewModel>();
            CreateMap<Subject, SubjectViewModel>();
            CreateMap<SubjectViewModel, Subject>();
        }
    }
}
