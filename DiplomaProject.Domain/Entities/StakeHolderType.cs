using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class StakeHolderType
    {
        public StakeHolderType()
        {
            StakeHolder = new HashSet<StakeHolder>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ProfessionName { get; set; }
        public double? Coefficient { get; set; }

        public ICollection<StakeHolder> StakeHolder { get; set; }
    }
}
