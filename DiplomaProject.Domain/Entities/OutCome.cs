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
        public int? OutComeTypeId { get; set; }
        public int? StakeHolderId { get; set; }
        public int? InitialSubjectId { get; set; }
        public int? ProfessionId { get; set; }
        public bool? IsNew { get; set; }

        public Profession Profession { get; set; }
        public StakeHolder StakeHolder { get; set; }
        public InitialSubject InitialSubject { get; set; }
        public OutComeType OutComeType { get; set; }
    }
}
