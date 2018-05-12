using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class SubjectModuleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }
        public List<SubjectViewModel> Subjects { get; set; }
    }
}
