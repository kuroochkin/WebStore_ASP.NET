var builder = WebApplication.CreateBuilder(args); // Создаем builder

var servises = builder.Services; // Добавляем сервисы
servises.AddControllersWithViews();// Добавляем MVC

var app = builder.Build(); // Сосздаем приложение


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Перехват всех ошибок
}

app.UseRouting(); // Добавляем маршрутизацию


// app.MapDefaultControllerRoute(); // Добавляем default-маршрут

app.MapControllerRoute( // Собственынй маршрут
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Запуск программы