using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Services;
using WebStore.Services.InCookies;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args); // ������� builder

var services = builder.Services; // ��������� �������

//services.AddScoped<IEmployeesData, SqlEmployeesData>(); 
//services.AddScoped<IProductData, SqlProductData>(); // !!! AddScoped !!!
services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IUsersData, SqlUsersData>();
//services.AddScoped<IOrderService, SqlOrderService>();

var configuration = builder.Configuration;
services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new(configuration["WebAPI"]));



services.AddIdentity<User, Role>()
   //.AddEntityFrameworkStores<WebStoreDB>()
   .AddDefaultTokenProviders(); // ���������� Identity

services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
   .AddTypedClient<IUserStore<User>, UsersClient>()
   .AddTypedClient<IUserRoleStore<User>, UsersClient>()
   .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
   .AddTypedClient<IUserEmailStore<User>, UsersClient>()
   .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
   .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
   .AddTypedClient<IUserClaimStore<User>, UsersClient>()
   .AddTypedClient<IUserLoginStore<User>, UsersClient>()
   .AddTypedClient<IRoleStore<Role>, RolesClient>();

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

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore.GB"; // ������ ��������
    opt.Cookie.HttpOnly = true; // �������� ������ �� Http

    //opt.Cookie.Expiration = TimeSpan.FromDays(10); // ��������
    opt.ExpireTimeSpan = TimeSpan.FromDays(10); // Cookie ������������� ������

    opt.LoginPath = "/Account/Login"; // ������ ����
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied"; // � ������ ���� �������� � �������

    opt.SlidingExpiration = true; // 
});

services.AddDbContext<WebStoreDB>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // ����������� �� � ��������
services.AddTransient<IDbInitializer, DbInitializer>(); // ������������ ������ ���������� ������ � ��

services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}
);// ��������� MVC

var app = builder.Build(); // ������� ����������

await using (var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: false).ConfigureAwait(true); //����� �������� ��������� �� ��������� �� �����
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // �������� ���� ������
}

app.UseStaticFiles(); // ���������� ����������� ����� (wwwroot)

app.UseMiddleware<TestMiddleware>(); // ��������� ���� ������������� ��

app.UseRouting(); // ��������� �������������

app.UseAuthentication(); 
app.UseAuthorization(); // ���������, ����� �� ������������ "���������" �� ������������

app.UseWelcomePage("/welcome");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute( // Areas(����� � �������)
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute( // ����������� �������
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

// app.MapDefaultControllerRoute(); // ��������� default-�������
app.Run(); // ������ ���������