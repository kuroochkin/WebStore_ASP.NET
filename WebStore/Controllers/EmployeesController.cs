using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        public IEmployeesData employees { get; set; } // Свойство для хранения сотрудников в контроллере через сервис
        public EmployeesController(IEmployeesData EmployeesData)
        {
            this.employees = EmployeesData; // Получаем данные по сотрудникам из класса TestData
        }

        public IActionResult Index()
        {
            return View(employees.GetAll());
        }

        public IActionResult Details(int Id)
        {
            var employee = employees.GetById(Id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Create() => View();
        public IActionResult Edit(int id)
        {
            var employee = employees.GetById(id);
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

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age,
            };

            if (!employees.Edit(employee))
                return NotFound();
                
            // Обработка модели
            return RedirectToAction("Index");
        }
           
        public IActionResult Delete(int id) => View();


    }
}
