using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Department
    {
        public Department()
        {
            Professions = new HashSet<Profession>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeadOfDepartment { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? FacultyId { get; set; }
        public byte? ExamMaxCount { get; set; }
        public byte? CreditMaxCount { get; set; }
        public byte? CourseWorkMaxCount { get; set; }
        public byte? TestMaxCount { get; set; }

        public Faculty Faculty { get; set; }
        public ICollection<Profession> Professions { get; set; }
    }
}
