using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Model
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;

        public WebStoreDBInitializer(WebStoreDB db)
        {
            _db = db;
        }
        public async Task InitializeAsync()
        {
            var pendingMigartions = await _db.Database.GetPendingMigrationsAsync();
            var appliedMigrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pendingMigartions.Any())
                await _db.Database.MigrateAsync();

            await InitializeProductAsync();
        }
        private async Task InitializeProductAsync()
        {
            await using(await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.Database.CommitTransactionAsync();
            }
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.Database.CommitTransactionAsync();
            }
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.Database.CommitTransactionAsync();
            }
        }
    }
}
