using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return Content("Добавление главного контроллера");
            return View(); // Первое представление
        }
    }
}
