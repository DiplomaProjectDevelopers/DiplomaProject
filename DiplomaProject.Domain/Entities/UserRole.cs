using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Domain.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        public int? ProfessionId { get; set; }

        public Profession Profession { get; set; }
    }
}
