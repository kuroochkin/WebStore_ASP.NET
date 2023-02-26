using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Services.Interfaces;
using WebStore.Services;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.InCookies;
using WebStore.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IEmployeesData, SqlEmployeesData>();
services.AddScoped<IProductData, SqlProductData>(); // !!! AddScoped !!!
services.AddScoped<IOrderService, SqlOrderService>();

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

services.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // Подключение БД к сервисам

services.AddTransient<IDbInitializer, DbInitializer>(); // Регистрируем сервис заполнения данных в бд

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
