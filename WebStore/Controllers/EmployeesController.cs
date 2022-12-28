using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        public ICollection<Employee> employees { get; set; }
        public EmployeesController()
        {
            this.employees = TestData.employees; // Получаем данные по сотрудникам из класса TestData
        }

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
