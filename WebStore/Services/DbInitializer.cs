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
            await InitializeEmployeesAsync(Cancel).ConfigureAwait(false); // Инициализация сотрудников

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

            var section_pool = TestData.Sections.ToDictionary(s => s.Id);
            var brands_pool = TestData.Brands.ToDictionary(b => b.Id);

            foreach(var child_section in TestData.Sections.Where(s => s.ParentId is not null))
                child_section.Parent = section_pool[(int)child_section.ParentId!];

            foreach (var product in TestData.Products)
            {
                product.Section = section_pool[product.SectionId]; // Устанавливаем родительскую секцию из пула секций

                if (product.BrandId is { } brand_id) // И устанавливаем Brand если он есть
                    product.Brand = brands_pool[brand_id];

                product.Id = 0; // Очищаем данные Id (EF установит сама!!!) 
                product.BrandId = null;
                product.SectionId = 0;
            }

            foreach(var section in TestData.Sections)
            {
                section.Id = 0; // Очищаем данные Id (EF установит сама!!!) 
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
                brand.Id = 0; // Очищаем данные Id (EF установит сама!!!) 

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);

                await _db.SaveChangesAsync(Cancel);
                
                await _db.Database.CommitTransactionAsync(Cancel);

            }
            

            _Logger.LogInformation("Инициализация тестовых данных БД выполнена успешно");
        }

        private async Task InitializeEmployeesAsync(CancellationToken Cancel)
        {
            if (await _db.Employees.AnyAsync(Cancel))
            {
                _Logger.LogInformation("Инициализация сотрудников не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация сотрудников...");
            await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

            TestData.employees.ForEach(employee => employee.Id = 0);

            await _db.Employees.AddRangeAsync(TestData.employees, Cancel);
            await _db.SaveChangesAsync(Cancel);

            await transaction.CommitAsync(Cancel);
            _Logger.LogInformation("Инициализация сотрудников выполнена успешно");
        }
    }
}
