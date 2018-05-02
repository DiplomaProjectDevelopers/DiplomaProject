using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Domain.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }
        public int ProfessionId { get; set; }
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        public Profession Profession { get; set; }
    }
}
