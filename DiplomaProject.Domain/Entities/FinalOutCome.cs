using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaProject.Domain.Entities
{
    public class FinalOutCome
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TotalWeight { get; set; }
        public int? OutComeTypeId { get; set; }
        public int? ProfessionId { get; set; }
        public bool? IsNew { get; set; }
        public int? SubjectId { get; set; }

        public int? InitialSubjectId { get; set; }

        public Subject Subject { get; set; }

        public Profession Profession { get; set; }

        public OutComeType OutComeType { get; set; }

        public InitialSubject InitialSubject { get; set; }

        public ICollection<Edge> LeftSideOutComes { get; set; }

        public ICollection<Edge> RightSideOutComes { get; set; }
    }
}
