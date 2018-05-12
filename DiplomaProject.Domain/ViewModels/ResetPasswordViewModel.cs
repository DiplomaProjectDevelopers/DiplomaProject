using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Էլ-փոստ դաշտը լրացված չէ")]
        [EmailAddress(ErrorMessage = "Էլ-փոստի ձևաչափը սխալ է:")]
        [Display(Name = "Էլ-փոստ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Գաղտնաբառ դաշտը լրացված չէ")]
        [Display(Name = "Գաղտնաբառ")]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Հաստատեք գաղտնաբառը")]
        [Compare("Password", ErrorMessage = "Գաղտնաբառը և Հաստատեք գաղտնաբառը դաշտերը չեն համընկնում")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
