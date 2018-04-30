using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Այս դաշտը պարտադիր է")]
        [Display(Name = "Մուտքանուն")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Այս դաշտը պարտադիր է")]
        [DataType(DataType.Password)]
        [Display(Name = "Գաղտնաբառ")]
        public string Password { get; set; }

        [Display(Name = "Հիշել")]
        public bool RememberMe { get; set; }
    }
}
