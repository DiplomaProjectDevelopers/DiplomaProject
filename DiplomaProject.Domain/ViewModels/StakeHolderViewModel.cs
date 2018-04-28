using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class StakeHolderViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
