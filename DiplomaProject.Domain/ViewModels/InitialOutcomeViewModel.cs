using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class InitialOutcomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public int? OutComeTypeId { get; set; }
        public int? InitialSubjectId { get; set; }
    }
}
