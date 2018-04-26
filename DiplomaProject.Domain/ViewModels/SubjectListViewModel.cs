using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class SubjectListViewModel
    {
        public SubjectListViewModel()
        {
            Subjects = new List<SubjectViewModel>();
        }
        public List<SubjectViewModel> Subjects { get; set; }
        public string ProfessionName { get; set; }
    }
}
