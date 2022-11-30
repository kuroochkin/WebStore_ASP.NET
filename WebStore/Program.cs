var builder = WebApplication.CreateBuilder(args); // Создаем builder
var app = builder.Build(); // Сосздаем приложение

var servises = builder.Services; // Добавляем сервисы
servises.AddControllersWithViews(); // Добавляем MVC

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Перехват всех ошибок
}


app.MapGet("/", () => app.Configuration["HelloProject"]);

app.Run(); // Запуск программы