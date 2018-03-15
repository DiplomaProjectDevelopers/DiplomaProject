using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class OutcomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TotalWeight { get; set; }
        public int? TypeId { get; set; }
        public int? ProfessionId { get; set; }
        public bool? IsNew { get; set; }
        public int? SubjectId { get; set; }
    }
}
