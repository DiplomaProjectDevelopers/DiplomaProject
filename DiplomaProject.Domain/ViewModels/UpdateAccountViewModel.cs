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

        [Display(Name = "Հեռախոսահամար")]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Այս դաշտը պարտադիր է լրացման համար")]
        [DataType(DataType.Password)]
        [Display(Name = "Հին գաղտնաբառ")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Նոր գաղտնաբառ")]
        [StringLength(32, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Հաստատեք նոր գաղտնաբառը")]
        [StringLength(32, MinimumLength = 6)]
        public string NewPasswordConfirm { get; set; }
    }
}
