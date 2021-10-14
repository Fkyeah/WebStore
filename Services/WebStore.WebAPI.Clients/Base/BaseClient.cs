using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient _httpClient { get; } 
        protected string _controllerAddress { get; }

        protected BaseClient(HttpClient httpClient, string controllerAddress)
        {
            _httpClient = httpClient;
            _controllerAddress = controllerAddress;
        }
    }
}
