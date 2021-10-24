using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient _httpClient { get; } 
        protected string _controllerAddress { get; }

        protected BaseClient(HttpClient httpClient, string controllerAddress)
        {
            _httpClient = httpClient;
            _controllerAddress = controllerAddress;
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>();
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await _httpClient.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await _httpClient.PutAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed) return;
            _Disposed = true;

            if (disposing)
            {
                // должны освободить управляемые ресурсы
            }

            // должны освободить неуправляемые ресурсы
        }
    }
}
