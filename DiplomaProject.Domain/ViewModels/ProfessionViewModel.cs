using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class ProfessionViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Անուն")]
        public string Name { get; set; }
        [Display(Name = "Նկարագրություն")]
        public string Description { get; set; }
        [Display(Name = "Ունի բակալավրի առկա կրթություն")]
        public bool? BdfullTime { get; set; }
        [Display(Name = "Կիսամյակներ")]
        public byte? BdfullTimeSemesters { get; set; }
        [Display(Name = "Ունի բակալավրի հեռակա կրթություն")]
        public bool? BdpartTime { get; set; }
        [Display(Name = "Կիսամյակներ")]
        public byte? BdpartTimeSemesters { get; set; }
        [Display(Name = "Ունի մագիստրատուրայի առկա կրթություն")]
        public bool? MdfullTime { get; set; }
        [Display(Name = "Կիսամյակներ")]
        public byte? MdfullTimeSemesters { get; set; }
        [Display(Name = "Ունի մագիստրատուրայի հեռակա կրթություն")]
        public bool? MdpartTime { get; set; }
        [Display(Name = "Կիսամյակներ")]
        public byte? MdpartTimeSemesters { get; set; }
        [Display(Name = "Ոլորտ")]
        public string Branch { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        [Display(Name = "Ամբիոն")]
        public string Department { get; set; }
    }
}
