using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Model;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.InMemory
{
    public class InMemoryEmployersData : IEmployersData
    {
        private readonly ILogger<InMemoryEmployersData> _logger;
        private int _currentMaxId;

        public InMemoryEmployersData(ILogger<InMemoryEmployersData> logger)
        {
            _logger = logger;
            _currentMaxId = TestData.EmployerList.Max(el => el.Id);
        }
        public int AddEmployer(Employer employer)
        {
            if (employer is null)
                throw new ArgumentNullException(nameof(employer));

            if (TestData.EmployerList.Contains(employer))
                return employer.Id;

            employer.Id = ++_currentMaxId;
            TestData.EmployerList.Add(employer);

            return employer.Id;
        }

        public bool DeleteEmployer(int id)
        {
            var employer = GetById(id);
            if (employer is null)
                return false;

            TestData.EmployerList.Remove(employer);
            return true;
        }

        public IEnumerable<Employer> GetAllEmployers()
        {
            return TestData.EmployerList;
        }

        public Employer GetById(int id)
        {
            var employer = TestData.EmployerList.FirstOrDefault(t => t.Id == id);

            if (employer is null)
                return null;

            return employer;
        }

        public void UpdateEmployer(Employer employer)
        {
            if (employer is null)
                throw new ArgumentNullException(nameof(employer));

            if (TestData.EmployerList.Contains(employer))
                return;

            var temp_employer = GetById(employer.Id);
            if (temp_employer is null)
                return;

            temp_employer.Name = employer.Name;
            temp_employer.LastName = employer.LastName;
            temp_employer.Patronymic = employer.Patronymic;
            temp_employer.Age = employer.Age;

        }
    }
}
