using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        public RolesClient(HttpClient httpClient) : base(httpClient, WebStore.Interfaces.WebAPI.Roles)
        {
        }
        #region IRoleStore<Role>

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync(_controllerAddress, role, cancel).ConfigureAwait(false);
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel).ConfigureAwait(false);

            return result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel)
        {
            var response = await PutAsync(_controllerAddress, role, cancel).ConfigureAwait(false);
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel).ConfigureAwait(false);

            return result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/Delete", role, cancel).ConfigureAwait(false);
            var result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel).ConfigureAwait(false);

            return result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetRoleId", role, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetRoleName", role, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetRoleNameAsync(Role role, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetRoleName/{name}", role, cancel).ConfigureAwait(false);
            role.Name = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetNormalizedRoleName", role, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetNormalizedRoleName/{name}", role, cancel).ConfigureAwait(false);
            role.NormalizedName = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<Role> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{_controllerAddress}/FindById/{id}", cancel)
               .ConfigureAwait(false);
        }

        public async Task<Role> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{_controllerAddress}/FindByName/{name}", cancel)
               .ConfigureAwait(false);
        }

        #endregion
    }
}
