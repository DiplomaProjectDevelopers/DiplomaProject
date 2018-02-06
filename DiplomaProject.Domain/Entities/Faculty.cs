using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Entities
{
    public class Faculty
    {
        public Faculty()
        {
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
