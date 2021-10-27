using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(HttpClient httpClient) : base(httpClient, WebStore.Interfaces.WebAPI.Users) 
        {
        }

        #region Implementation of IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/UserId", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/UserName", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/UserName/{name}", user, cancel);
            user.UserName = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/NormalUserName/", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/NormalUserName/{name}", user, cancel);
            user.NormalizedUserName = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/User", user, cancel);
            var creation_success = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return creation_success
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            var response = await PutAsync($"{_controllerAddress}/User", user, cancel);
            var update_result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return update_result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/User/Delete", user, cancel);
            var delete_result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return delete_result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_controllerAddress}/User/Find/{id}", cancel).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_controllerAddress}/User/Normal/{name}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{_controllerAddress}/Role/{role}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{_controllerAddress}/Role/Delete/{role}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/roles", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IList<string>>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/InRole/{role}", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{_controllerAddress}/UsersInRole/{role}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetPasswordHash", new PasswordHashDTO { User = user, Hash = hash }, cancel)
               .ConfigureAwait(false);
            user.PasswordHash = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel);
            //user.PasswordHash = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetPasswordHash", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/HasPassword", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetEmail/{email}", user, cancel).ConfigureAwait(false);
            user.Email = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetEmail", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetEmailConfirmed", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetEmailConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.EmailConfirmed = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_controllerAddress}/User/FindByEmail/{email}", cancel).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/User/GetNormalizedEmail", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetNormalizedEmail/{email}", user, cancel).ConfigureAwait(false);
            user.NormalizedEmail = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetPhoneNumber/{phone}", user, cancel).ConfigureAwait(false);
            user.PhoneNumber = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetPhoneNumber", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetPhoneNumberConfirmed", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetPhoneNumberConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.PhoneNumberConfirmed = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{_controllerAddress}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{_controllerAddress}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetLogins", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_controllerAddress}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetLockoutEndDate", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            var response = await PostAsync(
                    $"{_controllerAddress}/SetLockoutEndDate",
                    new SetLockoutDTO { User = user, LockoutEnd = EndDate },
                    cancel)
               .ConfigureAwait(false);
            user.LockoutEnd = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/IncrementAccessFailedCount", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<int>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            await PostAsync($"{_controllerAddress}/ResetAccessFailedCont", user, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetAccessFailedCount", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<int>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetLockoutEnabled", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetLockoutEnabled/{enabled}", user, cancel).ConfigureAwait(false);
            user.LockoutEnabled = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/SetTwoFactor/{enabled}", user, cancel).ConfigureAwait(false);
            user.TwoFactorEnabled = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetTwoFactorEnabled", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetClaims", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<Claim>>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                    $"{_controllerAddress}/AddClaims",
                    new AddClaimDTO { User = user, Claims = claims },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync(
                    $"{_controllerAddress}/ReplaceClaim",
                    new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                    $"{_controllerAddress}/RemoveClaims",
                    new RemoveClaimDTO { User = user, Claims = claims },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{_controllerAddress}/GetUsersForClaim", claim, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<User>>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion
    }
}