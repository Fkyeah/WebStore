using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Model;

namespace WebStore.Services.Interfaces
{
    interface IEmployersData
    {
        IEnumerable<Employer> GetAllEmployers();
        Employer GetById();
        int AddEmployer(Employer employer);
        void UpdateEmployer(int id, Employer employer);
        bool DeleteEmployer(int id);
    }
}
