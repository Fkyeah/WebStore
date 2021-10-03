﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Model
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDBInitializer> _logger;

        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> logger)
        {
            _db = db;
            _logger = logger;
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
                

            await InitializeProductAsync();
            _logger.LogInformation("Инициализация продуктов..");
        }
        private async Task InitializeProductAsync()
        {
            _logger.LogInformation("Добавление брендов..");
            await using(await _db.Database.BeginTransactionAsync())
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
    }
}
