using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public int? ProfessionId { get; set; }
    }
}
