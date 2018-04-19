using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Entities
{
    public class SubjectModule
    {
        public SubjectModule()
        {
            Subjects = new HashSet<Subject>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }
}
