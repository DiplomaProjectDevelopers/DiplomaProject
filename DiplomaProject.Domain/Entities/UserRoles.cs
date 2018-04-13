using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.Entities
{
    public class UserRoles : IdentityUserRole<string>
    {
        [Key]
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        [Key]
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }

    }
}
