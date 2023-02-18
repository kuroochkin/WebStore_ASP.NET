using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(
            WebStoreDB db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<DbInitializer> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _Logger = Logger;
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
            await InitializeIdentityAsync(Cancel).ConfigureAwait(false); // Инициализация Identity

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

        private async Task InitializeIdentityAsync(CancellationToken Cancel)
        {
            _Logger.LogInformation("Инициализация данных системы Identity");

            var timer = Stopwatch.StartNew();

            async Task CheckRole(string RoleName) // Метод проверяющий роль 
            {
                if (await _RoleManager.RoleExistsAsync(RoleName)) // Если роль найдена
                    _Logger.LogInformation("Роль {0} существует в БД. {1} c", RoleName, timer.Elapsed.TotalSeconds);
                else // Если роль не найдена, то создаем её
                {
                    _Logger.LogInformation("Роль {0} не существует в БД. {1} c", RoleName, timer.Elapsed.TotalSeconds);

                    await _RoleManager.CreateAsync(new Role { Name = RoleName }); 

                    _Logger.LogInformation("Роль {0} создана. {1} c", RoleName, timer.Elapsed.TotalSeconds);
                }
            }

            await CheckRole(Role.Administrators); //Используем метод для проверки наличия администратора
            await CheckRole(Role.Users); //Используем метод для проверки наличия обычного пользователя

            //Администратор обязательно должен быть!!!
            if (await _UserManager.FindByNameAsync(User.Administrator) is null) 
            {
                _Logger.LogInformation("Пользователь {0} отсутствует в БД. Создаю... {1} c", User.Administrator, timer.Elapsed.TotalSeconds);

                var admin = new User
                {
                    UserName = User.Administrator, // Присваиваем имя
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded) // Успех создания администратора
                {
                    _Logger.LogInformation("Пользователь {0} создан успешно. Наделяю его правами администратора... {1} c", User.Administrator, timer.Elapsed.TotalSeconds);

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("Пользователь {0} наделён правами администратора. {1} c", User.Administrator, timer.Elapsed.TotalSeconds);
                }
                else
                {
                    var errors = creation_result.Errors.Select(err => err.Description); 
                    _Logger.LogError("Учётная запись администратора не создана. Ошибки:{0}", string.Join(", ", errors)); // Нужен более сложный пароль!

                    throw new InvalidOperationException($"Невозможно создать пользователя {User.Administrator} по причине: {string.Join(", ", errors)}");
                }
            }

            _Logger.LogInformation("Данные системы Identity успешно добавлены в БД за {0} c", timer.Elapsed.TotalSeconds);
        }
    }
}
