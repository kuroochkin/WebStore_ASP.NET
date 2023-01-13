using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(WebStoreDB db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _Logger = logger;
        }
        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Инициализация БД...");

            if (RemoveBefore) //Если надо удалить БД, то удаляем
                await RemoveAsync(Cancel).ConfigureAwait(false);

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel);
            if (pending_migrations.Any())
            {
                _Logger.LogInformation("Выполнение миграции БД...");

                await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);

                _Logger.LogInformation("Выполнение миграции БД выполнено успешно");
            }


            await InitializeProductsAsync(Cancel).ConfigureAwait(false); // Инициализация товаров

            _Logger.LogInformation("Инициализация БД выполнена успешно");
        }

        public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Удаление БД...");
            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);

            if (result)
                _Logger.LogInformation("Удаление БД выполнено успешно");
            else
                _Logger.LogInformation("Удаление БД не требуется (отсутствует)");

            return result;
        }

        private async Task InitializeProductsAsync(CancellationToken Cancel)
        {
            if (_db.Sections.Any()) // Если у нас уже что-то лежит в бд в товарах
            {
                _Logger.LogInformation("Инициализация тестовых данных БД не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация тестовых данных БД ...");

            
            //Добавляем секции
            _Logger.LogInformation("Добавление секций в БД ...");

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel); // Добавляем все секции

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel); // Разрешаем себе добавлять самим Id
                await _db.SaveChangesAsync(Cancel); // Сохраняем бд
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel); // Запрещаем себе добавлять самим Id

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Добавление брендов в БД ...");
            //Добавляем бренды
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Добавление товаров в БД ...");
            //Добавляем товары
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }
            
            _Logger.LogInformation("Инициализация тестовых данных БД выполнена успешно");
        }
    }
}
