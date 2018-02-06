using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class InitialSubject
    {
        public InitialSubject()
        {
            InitialOutComes = new HashSet<InitialOutCome>();
            OutComes = new HashSet<OutCome>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProfessionId { get; set; }

        public Profession Profession { get; set; }
        public ICollection<InitialOutCome> InitialOutComes { get; set; }
        public ICollection<OutCome> OutComes { get; set; }
    }
}
