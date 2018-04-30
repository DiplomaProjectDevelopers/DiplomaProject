using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Entities
{
    public class UserProfessions
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProfessionId { get; set; }

        public User User { get; set; }

        public Profession Profession { get; set; }
    }
}
