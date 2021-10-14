using System.Collections.Generic;
using WebStore.Domain.Model;

namespace WebStore.Interfaces.Services
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
