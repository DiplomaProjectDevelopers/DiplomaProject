﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiplomaProject.Domain.Entities
{
    public class Role : IdentityRole<string>
    {
        [StringLength(500)]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string DisplayName { get; set; }
        public int? Priority { get; set; }
    }
}
