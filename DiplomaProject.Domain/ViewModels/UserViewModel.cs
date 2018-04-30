//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Professions = new List<ProfessionViewModel>();
        }
        public string Id { get; set; }
        [Display(Name = "Մուտքանուն")]
        public string Username { get; set; }
        [Display(Name = "Անուն")]
        public string FirstName { get; set; }
        [Display(Name = "Ազգանուն")]
        public string LastName { get; set; }
        [Display(Name ="Էլփոստ")]
        public string Email { get; set; }
        [Display(Name ="Հեռախոսահամար")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ծննդյան տարեթիվ")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Սեռ")]
        public bool? Gender { get; set; }
        [Display(Name = "Ներկայիս դեր")]
        public string CurrentRole { get; set; }
        [Display(Name= "Օգտատերի դեր")]
        public string SelectedRoleId { get; set; }

        public List<ProfessionViewModel> Professions { get; set; }
        public bool CanEdit { get; set; } = false;
    }
}
                                                                                                                                                                                                                                                  