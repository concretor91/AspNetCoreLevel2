using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services.Identity;

namespace WebStore.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        private const string address = WebAPI.Identity.Roles;
        public RolesClient(HttpClient client) : base(client) { }

        #region IRoleStore<Role>

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel) =>
            await (await PostAsync(address, role, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel) =>
            await (await PutAsync(address, role, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel) =>
            await (await PostAsync($"{address}/Delete", role, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel) =>
            await (await PostAsync($"{address}/GetRoleId", role, cancel))
               .Content
               .ReadAsAsync<string>(cancel);

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel) =>
            await (await PostAsync($"{address}/GetRoleName", role, cancel))
               .Content
               .ReadAsAsync<string>(cancel);

        public async Task SetRoleNameAsync(Role role, string name, CancellationToken cancel)
        {
            role.Name = name;
            await PostAsync($"{address}/SetRoleName/{name}", role, cancel);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel) =>
            await (await PostAsync($"{address}/GetNormalizedRoleName", role, cancel))
               .Content
               .ReadAsAsync<string>(cancel);

        public async Task SetNormalizedRoleNameAsync(Role role, string name, CancellationToken cancel)
        {
            role.NormalizedName = name;
            await PostAsync($"{address}/SetNormalizedRoleName/{name}", role, cancel);
        }

        public async Task<Role> FindByIdAsync(string id, CancellationToken cancel) =>
            await GetAsync<Role>($"{address}/FindById/{id}", cancel);

        public async Task<Role> FindByNameAsync(string name, CancellationToken cancel) =>
            await GetAsync<Role>($"{address}/FindByName/{name}", cancel);

        public void Dispose()
        {
        }

        #endregion
    }
}
