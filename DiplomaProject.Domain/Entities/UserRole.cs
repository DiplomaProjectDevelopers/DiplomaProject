using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiplomaProject.Domain.Entities
{
    public class UserRole : IdentityRole
    {
        public UserRole(string roleName) : base(roleName)
        {

        }
        public UserRole()
        {

        }
        [StringLength(500)]
        public override string Id { get => base.Id; set => base.Id = value; }
    }
}
