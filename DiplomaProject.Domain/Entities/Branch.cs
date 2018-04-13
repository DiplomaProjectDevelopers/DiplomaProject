using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Branch
    {
        public Branch()
        {
            StakeHolder = new HashSet<StakeHolder>();
            Professions = new HashSet<Profession>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Profession> Professions { get; set; }
        public ICollection<StakeHolder> StakeHolder { get; set; }
    }
}
