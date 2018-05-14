using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ամբիոնի անուն")]
        public string Name { get; set; }
        [Display(Name = "Նկարագրություն")]
        public string Description { get; set; }
        [Display(Name = "Ամբիոնի վարիչ")]
        public string HeadOfDepartment { get; set; }
        [Display(Name = "Հեռախոս")]
        public string Phone { get; set; }
        [Display(Name = "Էլ-փոստ")]
        public string Email { get; set; }
        public int? FacultyId { get; set; }
        [Display(Name = "Քննությունների առավելագույն քանակ")]
        public byte? ExamMaxCount { get; set; }
        [Display(Name = "Կրեդիտների մաքսիմալ քանակ")]
        public byte? CreditMaxCount { get; set; }
        [Display(Name = "Կուրսային աշխատանքների առավելագույն քանակ")]
        public byte? CourseWorkMaxCount { get; set; }
        [Display(Name = "Ստուգարքների առավելագույն քանակ")]
        public byte? TestMaxCount { get; set; }
    }
}
