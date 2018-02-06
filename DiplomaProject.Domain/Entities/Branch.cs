using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Branch
    {
        public Branch()
        {
            StakeHolder = new HashSet<StakeHolder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StakeHolder> StakeHolder { get; set; }
    }
}
