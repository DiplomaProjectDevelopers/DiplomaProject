using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UpdateAccountViewModel
    { 
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is reqired.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(32, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Confirm new password")]
        [StringLength(32, MinimumLength = 6)]
        public string NewPasswordConfirm { get; set; }
    }
}
