using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; }
    }
}
