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
        public bool? BdfullTime { get; set; }
        public byte? BdfullTimeSemesters { get; set; }
        public bool? BdpartTime { get; set; }
        public byte? BdpartTimeSemesters { get; set; }
        public bool? MdfullTime { get; set; }
        public byte? MdfullTimeSemesters { get; set; }
        public bool? MdpartTime { get; set; }
        public byte? MdpartTimeSemesters { get; set; }
        public int? DepartmentId { get; set; }
        public string AdminId { get; set; }
        [Display(Name = "Ամբիոն")]
        public string Department { get; set; }
    }
}
