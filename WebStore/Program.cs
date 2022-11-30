var builder = WebApplication.CreateBuilder(args); // Создаем builder

var servises = builder.Services; // Добавляем сервисы
servises.AddControllersWithViews();// Добавляем MVC

var app = builder.Build(); // Сосздаем приложение


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Перехват всех ошибок
}

app.UseRouting(); // Добавляем маршрутизацию



app.MapDefaultControllerRoute();

app.Run(); // Запуск программы