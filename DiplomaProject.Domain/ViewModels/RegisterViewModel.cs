using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Այդ դաշտը չի կարող դատարկ լինել")]
        [Display(Name = "Մուտքանուն *")]
        [StringLength(32, MinimumLength = 3)]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Էլփոստի սխալ ֆորմատ")]
        [Required(ErrorMessage = "Այդ դաշտը չի կարող դատարկ լինել")]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Գաղտնաբառ *")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "<<\"Գաղտնաբառ\" և \"Հաստատել գաղտնաբառը\" դաշտերի պարունակությունը չեն համընկնում")]
        [Display(Name = "Հաստատել գաղտնաբառը *")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Ծննդյան տարեթիվ")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1900-01-01", "2018-01-01")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Անուն")]
        [StringLength(40, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Ազգանուն")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Հեռախոսահամար")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Սեռ")]
        public bool? Gender { get; set; }
    }
}
