using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="This field is necessary")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage ="This field is necessary")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
