using WebStore.Infrastructure.Conventions;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); // Создаем builder

var servises = builder.Services; // Добавляем сервисы
servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - потому что InMemory!

servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}
);// Добавляем MVC

var app = builder.Build(); // Создаемм приложение


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Перехват всех ошибок
}

app.UseStaticFiles(); // Используем статические файлы (wwwroot)

app.UseMiddleware<TestConvention>(); // Добавляем свое промежуточное ПО

app.UseRouting(); // Добавляем маршрутизацию


// app.MapDefaultControllerRoute(); // Добавляем default-маршрут

app.MapControllerRoute( // Собственный маршрут
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Запуск программы