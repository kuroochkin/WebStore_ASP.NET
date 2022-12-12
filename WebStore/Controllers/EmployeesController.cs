using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> employees = new()
        {
            new Employee {Id = 1, LastName = "Келин", FirstName = "Кирилл", Patronymic = "Вячеславович", Age = 19},
            new Employee {Id = 2, LastName = "Горбатых", FirstName = "Александр", Patronymic = "Андреевич", Age = 18},
            new Employee {Id = 3, LastName = "Ярочевский", FirstName = "Максим", Patronymic = "Павлович", Age = 18}
        };
        public IActionResult Index()
        {
            return View(employees);
        }

        public IActionResult Details(int Id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == Id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }
    }
}
