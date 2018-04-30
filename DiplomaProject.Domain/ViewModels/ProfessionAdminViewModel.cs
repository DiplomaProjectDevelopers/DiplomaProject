using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class ProfessionAdminViewModel : UserViewModel
    {
        [Display(Name = "Ընտրեք մասնագիտությունը")]
        public int ProfessionId { get; set; }
    }
}
