using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class OutCome
    {
        public OutCome()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public int? TypeId { get; set; }
        public int? StakeholderId { get; set; }
        public int? SubjectId { get; set; }
        public int? ProfessionId { get; set; }
        public bool? IsNew { get; set; }

        public Profession Profession { get; set; }
        public StakeHolder Stakeholder { get; set; }
        public InitialSubject Subject { get; set; }
        public OutComeType Type { get; set; }
    }
}
