using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.Model
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDBInitializer> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> logger, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            var pendingMigartions = await _db.Database.GetPendingMigrationsAsync();
            var appliedMigrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pendingMigartions.Any())
            {
                _logger.LogInformation("Запуск миграций..");
                await _db.Database.MigrateAsync();
            }

            try
            {
                _logger.LogInformation("Инициализация продуктов..");
                await InitializeProductAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Инициализация продуктов завершилась неудачно");
                throw;
            }
            try
            {
                _logger.LogInformation("Инициализация системы Identity..");
                await InitializeIdentityAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Инициализация системы Identity завершилась неудачно");
                throw;
            }
        }
        private async Task InitializeProductAsync()
        {
            if (_db.Brands.Any())
            {
                _logger.LogInformation("Инициализация не требуется и уже была произведена ранее");
                return;
            }
            _logger.LogInformation("Добавление брендов..");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Добавление брендов прошло успешно..");
            _logger.LogInformation("Добавление секций..");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Добавление секций прошло успешно..");
            _logger.LogInformation("Добавление продуктов..");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Добавление продуктов прошло успешно..");
        }

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string roleName)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                    _logger.LogInformation("Роль уже существует: {0}", roleName);
                else
                {
                    _logger.LogInformation("Роль не существует: {0}", roleName);
                    await _roleManager.CreateAsync(new Role { Name = roleName });
                    _logger.LogInformation("Роль успешно создана: {0}", roleName);
                }
                              
            }
            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation("Пользователя {0} не существует!", User.Administrator);
                _logger.LogInformation("Создаем..");
                var admin = new User
                { 
                    UserName = User.Administrator,
                };
                
                var creationResult = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if(creationResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} успешно создан", User.Administrator);
                    await _userManager.AddToRoleAsync(admin, Role.Administrators);
                    _logger.LogInformation("Роль выдана администратору");
                }
                else
                {
                    _logger.LogError("Ошибка при создании пользователя {0}. Ошибки {1}", User.Administrator, string.Join(", ", creationResult.Errors));
                    throw new InvalidOperationException($"Ошибка при создании пользователя {User.Administrator}. Ошибки {string.Join(", ", creationResult.Errors)}");
                }
                _logger.LogInformation("Данные Identity успешно добавлены в БД");
            }
            else
            {
                _logger.LogInformation("Данные администратора уже были добавлены в БД ранее");
            }
            
        }
    }
}
