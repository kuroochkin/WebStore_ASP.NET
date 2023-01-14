using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register() // Регистрация
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register()//EmployeeViewModel model)
        {

        }


        public IActionResult Login() => View(); // Вход в систему
        public IActionResult Logout() => RedirectToAction("Index", "Home"); // Выход из системы
        public IActionResult AccessDenied() => View(); // Отказ в доступе
    }
}
