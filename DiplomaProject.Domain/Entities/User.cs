using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Domain.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            Professions = new HashSet<Profession>();
        }

        [StringLength(500)]
        public override string Id { get => base.Id  ; set => base.Id = value; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? RoleId { get; set; }

        public UserRole Role { get; set; }
        public ICollection<Profession> Professions { get; set; }
    }
}
