using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); // Создаем builder

var services = builder.Services; // Добавляем сервисы
services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - потому что InMemory
services.AddScoped<IProductData, SqlProductData>(); // !!! AddScoped !!!

services.AddIdentity<User, Role>()
   .AddEntityFrameworkStores<WebStoreDB>()
   .AddDefaultTokenProviders(); // Подключаем Identity

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG // условная компиляция
    opt.Password.RequireDigit = false; // Пароль можно без цифр
    opt.Password.RequireLowercase = false; // Требования к нижнему регистру
    opt.Password.RequireUppercase = false; // Требования к верхнему регистру
    opt.Password.RequireNonAlphanumeric = false; // Отключаем неалфавитные символы
    opt.Password.RequiredLength = 3; // Минимальная длина - 3
    opt.Password.RequiredUniqueChars = 3; // Как минимум три уникальных символа
#endif

    opt.User.RequireUniqueEmail = false; // Не нужны уникальные email
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
    //Разрешенные символы 
    opt.Lockout.AllowedForNewUsers = false; // Отключаем автоматическую блокировку зарегистрированного пользователя
    opt.Lockout.MaxFailedAccessAttempts = 10; // 10 попыток "угадать" пароль
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Если 10 попыток неверны, то блокируем попытки на 15 минут
});

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore.GB"; // Задаем название
    opt.Cookie.HttpOnly = true; // Передаем только по Http

    //opt.Cookie.Expiration = TimeSpan.FromDays(10); // устарело
    opt.ExpireTimeSpan = TimeSpan.FromDays(10); // Cookie запрашивается заново

    opt.LoginPath = "/Account/Login"; // Задаем путь
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied"; // В случае если отказано в доступе

    opt.SlidingExpiration = true; // 
});

services.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // Подключение БД к сервисам
services.AddTransient<IDbInitializer, DbInitializer>(); // Регистрируем сервис заполнения данных в бд

services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}
);// Добавляем MVC

var app = builder.Build(); // Создаем приложение

await using (var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: false); //Перед запуском программы бд удаляться не будет
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Перехват всех ошибок
}

app.UseStaticFiles(); // Используем статические файлы (wwwroot)

app.UseMiddleware<TestMiddleware>(); // Добавляем свое промежуточное ПО

app.UseRouting(); // Добавляем маршрутизацию

app.UseAuthentication(); 
app.UseAuthorization(); // Проверяет, може тли пользователь "добраться" до контроллеров

app.UseWelcomePage("/welcome");


// app.MapDefaultControllerRoute(); // Добавляем default-маршрут

app.MapControllerRoute( // Собственный маршрут
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Запуск программы