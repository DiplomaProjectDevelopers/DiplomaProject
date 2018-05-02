using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaProject.Domain.Entities
{
    public class User : IdentityUser<string>
    {
        [StringLength(500)]
        public override string Id { get => base.Id  ; set => base.Id = value; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
