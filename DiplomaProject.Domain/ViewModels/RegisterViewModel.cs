using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field can't be empty")]
        [Display(Name = "Username *")]
        [StringLength(32, MinimumLength =3)]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage ="Incorrect email format")]
        [Required(ErrorMessage ="This field can't be empty")]
        [Display(Name ="Email *")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Password *")]
        [StringLength(32,  MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "The fields Password and Confirm Password are different.")]
        [Display(Name = "Confirm password *")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Date of birth")]
        [DataType(DataType.Date)]
        [Range(typeof (DateTime),"1900-01-01","2018-01-01")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "First Name")]
        [StringLength(40, MinimumLength =2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength =2)]
        public string LastName { get; set; }
    }
}
