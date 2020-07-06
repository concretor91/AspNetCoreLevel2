using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services.Identity;

namespace WebStore.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        string address = WebAPI.Identity.Users;
        public UsersClient(HttpClient client) : base(client) { }

        #region Implementation of IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/UserId", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/UserName", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
        {
            user.UserName = name;
            await PostAsync($"{address}/UserName/{name}", user, cancel);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/NormalUserName/", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
        {
            user.NormalizedUserName = name;
            await PostAsync($"{address}/NormalUserName/{name}", user, cancel);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var creation_success = await (await PostAsync($"{address}/User", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);

            return creation_success
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            return await (await PutAsync($"{address}/User", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/User/Delete", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<User>($"{address}/User/Find/{id}", cancel);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<User>($"{address}/User/Normal/{name}", cancel);
        }

        #endregion

        #region Implementation of IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{address}/Role/{role}", user, cancel);
        }

        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{address}/Role/Delete/{role}", user, cancel);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/roles", user, cancel))
               .Content
               .ReadAsAsync<IList<string>>(cancel);
        }

        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/InRole/{role}", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{address}/UsersInRole/{role}", cancel);
        }

        #endregion

        #region Implementation of IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
        {
            user.PasswordHash = hash;
            await PostAsync(
                $"{address}/SetPasswordHash", new PasswordHashDTO { User = user, Hash = hash },
                cancel);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetPasswordHash", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/HasPassword", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            user.Email = email;
            await PostAsync($"{address}/SetEmail/{email}", user, cancel);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetEmail", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetEmailConfirmed", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.EmailConfirmed = confirmed;
            await PostAsync($"{address}/SetEmailConfirmed/{confirmed}", user, cancel);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<User>($"{address}/User/FindByEmail/{email}", cancel);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/User/GetNormalizedEmail", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
        {
            user.NormalizedEmail = email;
            await PostAsync($"{address}/SetNormalizedEmail/{email}", user, cancel);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
        {
            user.PhoneNumber = phone;
            await PostAsync($"{address}/SetPhoneNumber/{phone}", user, cancel);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetPhoneNumber", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetPhoneNumberConfirmed", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.PhoneNumberConfirmed = confirmed;
            await PostAsync($"{address}/SetPhoneNumberConfirmed/{confirmed}", user, cancel);
        }

        #endregion

        #region Implementation of IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{address}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel);
        }

        public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{address}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetLogins", user, cancel))
               .Content
               .ReadAsAsync<List<UserLoginInfo>>(cancel);
        }

        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{address}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel);
        }

        #endregion

        #region Implementation of IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetLockoutEndDate", user, cancel))
               .Content
               .ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            user.LockoutEnd = EndDate;
            await PostAsync(
                $"{address}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate },
                cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/IncrementAccessFailedCount", user, cancel))
               .Content
               .ReadAsAsync<int>(cancel);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            await PostAsync($"{address}/ResetAccessFailedCont", user, cancel);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetAccessFailedCount", user, cancel))
               .Content
               .ReadAsAsync<int>(cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetLockoutEnabled", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            user.LockoutEnabled = enabled;
            await PostAsync($"{address}/SetLockoutEnabled/{enabled}", user, cancel);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            user.TwoFactorEnabled = enabled;
            await PostAsync($"{address}/SetTwoFactor/{enabled}", user, cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetTwoFactorEnabled", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetClaims", user, cancel))
               .Content
               .ReadAsAsync<List<Claim>>(cancel);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                $"{address}/AddClaims",
                new AddClaimDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync(
                $"{address}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim },
                cancel);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                $"{address}/RemoveClaims",
                new RemoveClaimDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            return await (await PostAsync($"{address}/GetUsersForClaim", claim, cancel))
               .Content
               .ReadAsAsync<List<User>>(cancel);
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
