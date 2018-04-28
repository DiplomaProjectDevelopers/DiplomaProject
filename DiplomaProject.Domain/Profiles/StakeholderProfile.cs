using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Profiles
{
    class StakeholderProfile : Profile
    {
        public StakeholderProfile()
        {
            CreateMap<StakeHolder, StakeHolderViewModel>();
            CreateMap<StakeHolderViewModel, StakeHolder>();
        }
    }
}
