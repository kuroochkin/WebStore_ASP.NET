using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); // Создаем builder

var servises = builder.Services; // Добавляем сервисы
servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - потому что InMemory
servises.AddSingleton<IProductData, InMemoryProductData>(); // Singleton - потому что InMemory

servises.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // Подключение БД к сервисам
servises.AddTransient<IDbInitializer, DbInitializer>(); // Регистрируем сервис заполнения данных в бд

servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}
);// Добавляем MVC

var app = builder.Build(); // Создаемм приложение

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

app.UseWelcomePage("/welcome");


// app.MapDefaultControllerRoute(); // Добавляем default-маршрут

app.MapControllerRoute( // Собственный маршрут
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Запуск программы