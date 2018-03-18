using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        public string Username { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name ="Email Address")]
        public string Email { get; set; }
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Date of birth")]
        public DateTime? BirthDate { get; set; }
        public bool? Gender { get; set; }
        [Display(Name = "Current Role")]
        public List<string> CurrentRoles { get; set; }
        [Required(ErrorMessage = "Please select a role before submitting.")]
        [Display(Name = "Please select new role for current user.")]
        public string SelectedRoleId { get; set; }
    }
}
