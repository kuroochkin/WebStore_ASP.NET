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
   .AddDefaultTokenProviders(); // ���������� Identity

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG // �������� ����������
    opt.Password.RequireDigit = false; // ������ ����� ��� ����
    opt.Password.RequireLowercase = false; // ���������� � ������� ��������
    opt.Password.RequireUppercase = false; // ���������� � �������� ��������
    opt.Password.RequireNonAlphanumeric = false; // ��������� ������������ �������
    opt.Password.RequiredLength = 3; // ����������� ����� - 3
    opt.Password.RequiredUniqueChars = 3; // ��� ������� ��� ���������� �������
#endif

    opt.User.RequireUniqueEmail = false; // �� ����� ���������� email
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
    //����������� ������� 
    opt.Lockout.AllowedForNewUsers = false; // ��������� �������������� ���������� ������������������� ������������
    opt.Lockout.MaxFailedAccessAttempts = 10; // 10 ������� "�������" ������
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // ���� 10 ������� �������, �� ��������� ������� �� 15 �����
});

services.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // ����������� �� � ��������

services.AddTransient<IDbInitializer, DbInitializer>(); // ������������ ������ ���������� ������ � ��

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
