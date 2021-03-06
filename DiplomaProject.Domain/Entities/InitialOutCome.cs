﻿using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class InitialOutCome
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public int? OutComeTypeId { get; set; }
        public int? InitialSubjectId { get; set; }

        public InitialSubject InitialSubject { get; set; }
        public OutComeType OutComeType { get; set; }
    }
}
