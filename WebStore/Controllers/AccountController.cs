using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register() => View(); // Регистрация
        public IActionResult Login() => View(); // Вход в систему
        public IActionResult Logout() => RedirectToAction("Index", "Home"); // Выход из системы
        public IActionResult AccessDehied() => View(); // Отказ в доступе
    }
}
