using System.Collections.Generic;
using WebStore.Model;

namespace WebStore.Services.Interfaces
{
    public interface IEmployersData
    {
        IEnumerable<Employer> GetAllEmployers();
        Employer GetById(int id);
        int AddEmployer(Employer employer);
        void UpdateEmployer(Employer employer);
        bool DeleteEmployer(int id);
    }
}
