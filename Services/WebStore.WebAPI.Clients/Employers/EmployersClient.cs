using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain.Model;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employers
{
    public class EmployersClient : BaseClient, IEmployersData
    {
        public EmployersClient(HttpClient httpClient) : base(httpClient, "api/employers")
        {

        }

        public IEnumerable<Employer> GetAllEmployers()
        {
            var result = Get<IEnumerable<Employer>>(_controllerAddress);
            return result;
        }

        public Employer GetById(int id)
        {
            var result = Get<Employer>($"{_controllerAddress}/{id}");
            return result;
        }

        public int AddEmployer(Employer employer)
        {
            var response = Post(_controllerAddress, employer);
            var addedEmployer = response.Content.ReadFromJsonAsync<Employer>();
            return addedEmployer.Id;
        }

        public void UpdateEmployer(Employer employer)
        {
            Put(_controllerAddress, employer);
        }

        public bool DeleteEmployer(int id)
        {
            var response = Delete($"{_controllerAddress}/{id}");
            var result = response.IsSuccessStatusCode;
            return result;
        }
    }
}
