using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaProject.Domain.Entities
{
    public class Edge
    {
        public int Id { get; set; }
        public int? LeftOutComeId { get; set; }
        public int? RightOutComeId { get; set; }

        public int ProfessionId { get; set; }
        public FinalOutCome LeftOutCome { get; set; }
        public FinalOutCome RightOutCome { get; set; }
    }
}
