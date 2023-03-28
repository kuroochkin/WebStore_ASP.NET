using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebStore.DAL.Migrations;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class EmployeesController : Controller
    {
        public readonly IEmployeesData _EmployessData; // Свойство для хранения сотрудников в контроллере через сервис
        
        public readonly IMapper _Mapper;
        
        public EmployeesController(IEmployeesData EmployeesData, IMapper Mapper)
        {
            _EmployessData = EmployeesData; // Получаем данные по сотрудникам из класса TestData
            _Mapper = Mapper;
        }

        public IActionResult Index()
        {
            return View(_EmployessData.GetAll());
        }

        public IActionResult Details(int Id)
        {
            var employee = _EmployessData.GetById(Id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        [Authorize(Roles = Role.Administrators)] // Чтобы редактировать сотрудников нужно быть админом
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _EmployessData.GetById((int)id);
            if (employee is null)
                return NotFound();

            //var model = new EmployeeViewModel
            //{
            //    Id = employee.Id,
            //    LastName = employee.LastName,
            //    Name = employee.FirstName,
            //    Patronymic = employee.Patronymic,
            //    Age = employee.Age,
            //};

            var model = _Mapper.Map<EmployeeViewModel>(employee);
            
            return View(model); // Отправка модели на обработку
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            //var employee = new Employee
            //{
            //    Id = model.Id,
            //    LastName = model.LastName,
            //    FirstName = model.Name,
            //    Patronymic = model.Patronymic,
            //    Age = model.Age,
            //};

			var employee = _Mapper.Map<Employee>(model);

			if (model.Id == 0)
                _EmployessData.Add(employee);
            else if (!_EmployessData.Edit(employee))
                return NotFound();
                
            // Обработка модели
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if(id < 0)
                return BadRequest();
            
            var employee = _EmployessData.GetById(id);
            if (employee is null)
                return NotFound();

			//var model = new EmployeeViewModel
			//{
			//    Id = employee.Id,
			//    LastName = employee.LastName,
			//    Name = employee.FirstName,
			//    Patronymic = employee.Patronymic,
			//    Age = employee.Age,
			//};

			var model = _Mapper.Map<EmployeeViewModel>(employee);

			return View(model); // Отправка модели на обработку
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _EmployessData.GetById(id);
            if (employee is null)
                return NotFound();

            _EmployessData.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
