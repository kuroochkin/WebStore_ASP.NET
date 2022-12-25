using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return Content("Добавление главного контроллера");
            return View(); // Первое представление
        }

        public IActionResult Contacts() => View();
        
    }
}
