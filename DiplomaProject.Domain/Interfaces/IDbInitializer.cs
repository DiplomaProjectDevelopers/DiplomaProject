using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Interfaces
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}
