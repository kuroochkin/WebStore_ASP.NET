using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> employees = new()
        {
            new Employee {Id = 1, LastName = "Келин", FirstName = "Кирилл", Patronymic = "Вячеславович", Age = 19},
            new Employee {Id = 2, LastName = "Горбатых", FirstName = "Александр", Patronymic = "Андреевич", Age = 18},
            new Employee {Id = 3, LastName = "Ярочевский", FirstName = "Максим", Patronymic = "Павлович", Age = 18}
        };

        public IActionResult Index()
        {
            //return Content("Добавление главного контроллера");
            return View(); // Первое представление
        }

        public string ConfiguredAction(string id)
        {
            return $"Hello World! {id}";
        }

        public IActionResult Employees()
        {
            return View(employees);
        }
    }
}
