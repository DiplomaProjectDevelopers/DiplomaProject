using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.ViewModels
{
    public class EdgeViewModel
    {
        public int Id { get; set; }

        public int FromNode { get; set; }

        public int ToNode { get; set; }
    }
}
