using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class StakeHolderViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Անուն")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Ազգանուն")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Տիպ")]
        [Required]
        public int? TypeId { get; set; }
        [Display(Name = "Տիպ")]
        public string TypeName { get; set; }
        [Display(Name = "Էլփոստ")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Հեռախոսահամար")]
        public string Phone { get; set; }
        [Display(Name = "Ընկերություն")]
        public string CompanyName { get; set; }
        [Display(Name = "Ոլորտ")]
        [Required]
        public int? BranchId { get; set; }
        [Display(Name = "Ոլորտ")]
        public string BranchName { get; set; }

        public bool CanEdit { get; set; } = false;
    }
}
