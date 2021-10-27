using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValueClient
    {
        public ValuesClient(HttpClient httpClient) : base(httpClient, WebStore.Interfaces.WebAPI.Values)
        {

        }
        public void Add(string value) 
        {
            var response = _httpClient.PostAsJsonAsync($"{_controllerAddress}/Add", value).Result;
            response.EnsureSuccessStatusCode();
        }

        public int Count()
        {
            var response = _httpClient.GetAsync($"{_controllerAddress}/Count").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<int>().Result;

            return -1;
        }

        public bool Delete(int id)
        {
            var response = _httpClient.DeleteAsync($"{_controllerAddress}/Delete/{id}").Result;
            return response.IsSuccessStatusCode;
        }

        public void Edit(int id, string value)
        {
            var response = _httpClient.PostAsJsonAsync($"{_controllerAddress}/UpdateValue/{id}", value).Result;
            response.EnsureSuccessStatusCode();
        }

        public IEnumerable<string> GetAll()
        {
            var response = _httpClient.GetAsync(_controllerAddress).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();
        }

        public string GetById(int id)
        {
            var response = _httpClient.GetAsync($"{_controllerAddress}/GetById/{id}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;

            return null;
        }
    }
}
