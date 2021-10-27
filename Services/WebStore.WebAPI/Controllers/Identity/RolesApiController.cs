using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebStore.Interfaces.WebAPI.Roles)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;
        private readonly ILogger<RolesApiController> _logger;

        public RolesApiController(WebStoreDB db, ILogger<RolesApiController> logger)
        {
            _roleStore = new RoleStore<Role>(db);
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAll() => await _roleStore.Roles.ToArrayAsync();
        [HttpPost]
        public async Task<bool> CreateAsync(Role role)
        {
            _logger.LogInformation("Запрос на создание роли {0}", role.Name);

            var creation_result = await _roleStore.CreateAsync(role);
            if (!creation_result.Succeeded)
            {
                foreach(var error in creation_result.Errors)
                {
                    _logger.LogWarning("Ошибка при создании роли: {0}", error.Description);
                }
            }

            _logger.LogInformation("Роль {0} успешно создана", role.Name);

            return creation_result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(Role role)
        {
            _logger.LogInformation("Запрос на изменение роли {0}", role.Name);

            var uprate_result = await _roleStore.UpdateAsync(role);
            if (!uprate_result.Succeeded)
            {
                foreach (var error in uprate_result.Errors)
                {
                    _logger.LogWarning("Ошибка при изменении роли: {0}", error.Description);
                }
            }

            _logger.LogInformation("Роль {0} успешно изменена", role.Name);

            return uprate_result.Succeeded;
        }

        [HttpDelete]
        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(Role role)
        {
            _logger.LogInformation("Запрос на удаление роли {0}", role.Name);

            var delete_result = await _roleStore.DeleteAsync(role);
            if (!delete_result.Succeeded)
            {
                foreach (var error in delete_result.Errors)
                {
                    _logger.LogWarning("Ошибка при удалении роли: {0}", error.Description);
                }
            }

            _logger.LogInformation("Роль {0} успешно удалена", role.Name);

            return delete_result.Succeeded;
        }

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role) => await _roleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role) => await _roleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync(Role role, string name)
        {
            _logger.LogInformation("Запрос на изменение роли {0}. Новое значение - {1}", role.Name, name);

            await _roleStore.SetRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);

            _logger.LogInformation("Роль успешно обновлена");
            return role.Name;
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await _roleStore.GetNormalizedRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
        {
            _logger.LogInformation("Запрос на иземнение нормализованного имени роли {0}. Новое значение - {1}", role.Name, name);

            await _roleStore.SetNormalizedRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
            return role.NormalizedName;
        }

        [HttpGet("FindById/{id}")]
        public async Task<Role> FindByIdAsync(string id) => await _roleStore.FindByIdAsync(id);

        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name) => await _roleStore.FindByNameAsync(name);
    }
}