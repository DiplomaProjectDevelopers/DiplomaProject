using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UserViewModel
    {
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

        [Display(Name = "Current Roles")]
        public List<string> CurrentRoles { get; set; }

        public RoleViewModel Role { get; set; }
    }
}
