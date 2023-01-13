using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); // ������� builder

var servises = builder.Services; // ��������� �������
servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - ������ ��� InMemory
servises.AddSingleton<IProductData, InMemoryProductData>(); // Singleton - ������ ��� InMemory

servises.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // ����������� �� � ��������

servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}
);// ��������� MVC

var app = builder.Build(); // �������� ����������


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // �������� ���� ������
}

app.UseStaticFiles(); // ���������� ����������� ����� (wwwroot)

app.UseMiddleware<TestMiddleware>(); // ��������� ���� ������������� ��

app.UseRouting(); // ��������� �������������

app.UseWelcomePage("/welcome");


// app.MapDefaultControllerRoute(); // ��������� default-�������

app.MapControllerRoute( // ����������� �������
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // ������ ���������