using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
   public class StakeHolderTypeViewModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ProfessionName { get; set; }
        public double? Coefficient { get; set; }
    }
}
