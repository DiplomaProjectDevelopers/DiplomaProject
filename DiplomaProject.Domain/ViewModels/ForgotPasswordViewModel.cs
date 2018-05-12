using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Էլ-փոստ դաշտը լրացված չէ")]
        [EmailAddress(ErrorMessage = "Էլ-փոստի սխալ ձևաչափ")]
        [Display(Name = "Էլ-փոստ")]
        public string Email { get; set; }
    }
}
