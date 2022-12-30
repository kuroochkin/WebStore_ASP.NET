using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        public ICollection<Employee> employees { get; set; } // Свойство для хранения сотрудников в контроллере
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

        public IActionResult Create() => View();
        public IActionResult Edit(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeEditViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };
            
            return View(model); // Отправка модели на обработку
        }

        public IActionResult Edit(EmployeeEditViewModel model)
        {
            // Обработка модели
            return RedirectToAction("Index");
        }
           
        public IActionResult Delete(int id) => View();


    }
}
