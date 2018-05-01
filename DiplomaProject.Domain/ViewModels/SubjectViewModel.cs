using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
   public class SubjectViewModel
    {
        public SubjectViewModel()
        {
            Outcomes = new List<OutcomeViewModel>();
            DependentSubjects = new List<int>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Անուն")]
        public string Name { get; set; }
        [Display(Name = "Մասնագիտություն")]
        public int? ProfessionId { get; set; }
        [Required]
        [Display(Name = "Առարկայական մոդուլ")]
        public int? SubjectModuleId { get; set; }

        [Display(Name = "Մոդուլ")]
        public string SubjectModule { get; set; }

        [Display(Name = "Կրեդիտ")]
        public int? Credit { get; set; }
        [Display(Name = "Կիսամյակ")]
        public int? Level { get; set; }

        [Display(Name = "Ընդհանուր ժամաքանակ")]
        public int? TotalHours { get; set; }

        public List<OutcomeViewModel> Outcomes { get; set; }

        public List<int> DependentSubjects { get; set; }
    }
}
