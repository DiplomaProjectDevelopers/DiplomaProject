using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class StakeHolder
    {
        public StakeHolder()
        {
            OutCome = new HashSet<OutCome>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TypeId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public int? BranchId { get; set; }

        public Branch Branch { get; set; }
        public StakeHolderType Type { get; set; }
        public ICollection<OutCome> OutCome { get; set; }
    }
}
