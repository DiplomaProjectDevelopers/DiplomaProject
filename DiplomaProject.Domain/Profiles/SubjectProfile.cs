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
            CreateMap<Subject, SubjectViewModel>().ForMember(s => s.SubjectModuleId, opt => opt.MapFrom(m => m.ModuleId));
            CreateMap<SubjectViewModel, Subject>().ForMember(m => m.ModuleId, opt => opt.MapFrom(s => s.SubjectModuleId));
        }
    }
}
