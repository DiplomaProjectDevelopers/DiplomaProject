using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Subject
    {
        public Subject()
        {
            FinalOutComes = new HashSet<FinalOutCome>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? TotalHours { get; set; }
        public int? LabHours { get; set; }
        public int? LectionHours { get; set; }
        public int? PracticalHours { get; set; }
        public int? CourseHourse { get; set; }
        public int? ProfessionId { get; set; }
        public int? ModuleId { get; set; }
        public int? Credit { get; set; }

        public Profession Profession { get; set; }

        public SubjectModule SubjectModule { get; set; }
        public ICollection<FinalOutCome> FinalOutComes { get; set; }
    }
}
