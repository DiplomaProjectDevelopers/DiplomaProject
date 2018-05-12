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
        public ProfessionViewModel Profession { get; set; }
        public int? Credit { get; set; }
        public int? TotalHours { get; set; }
        public int? CourseHourse { get; set; }
        public int? LectionHours { get; set; }
        public int? LabHours { get; set; }

    }
}
