using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class InitialOutCome
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public int? TypeId { get; set; }
        public int? SubjectId { get; set; }

        public InitialSubject Subject { get; set; }
        public OutComeType Type { get; set; }
    }
}
