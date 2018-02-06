using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class ProfessionAdminViewModel : UserViewModel
    {
        public List<ProfessionViewModel> Professions { get; set; }
    }
}
