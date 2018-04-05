using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Profiles
{
    public class OutcomeProfile : Profile
    {
        public OutcomeProfile()
        {
            CreateMap<FinalOutCome, OutcomeViewModel>();
            CreateMap<EdgeViewModel, Edge>().ForMember(edge => edge.LeftOutComeId, opt => opt.MapFrom(src => src.FromNode))
                .ForMember(edge => edge.RightOutComeId, opt => opt.MapFrom(src => src.ToNode));

        }
    }
}
