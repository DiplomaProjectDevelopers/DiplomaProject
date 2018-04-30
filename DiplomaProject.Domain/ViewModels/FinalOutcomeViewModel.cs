using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.ViewModels
{
    public class FinalOutcomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int? OutComeTypeId { get; set; }
        public int? StakeHolderId { get; set; }
        public int? InitialSubjectId { get; set; }
        public int? ProfessionId { get; set; }
        public bool? IsNew { get; set; }
    }
}
