using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Profession
    {
        public Profession()
        {
            InitialSubject = new HashSet<InitialSubject>();
            OutCome = new HashSet<OutCome>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? BdfullTime { get; set; }
        public byte? BdfullTimeSemesters { get; set; }
        public bool? BdpartTime { get; set; }
        public byte? BdpartTimeSemesters { get; set; }
        public bool? MdfullTime { get; set; }
        public byte? MdfullTimeSemesters { get; set; }
        public bool? MdpartTime { get; set; }
        public byte? MdpartTimeSemesters { get; set; }
        public int? DepartmentId { get; set; }
        public string AdminId { get; set; }

        public User Admin { get; set; }
        public Department Department { get; set; }
        public ICollection<InitialSubject> InitialSubject { get; set; }
        public ICollection<OutCome> OutCome { get; set; }
    }
}
