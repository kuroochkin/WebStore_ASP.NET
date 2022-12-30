using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private ICollection<Employee> _employees;
        private int _MaxFreeId; // Максимальный свободный ID
        public InMemoryEmployeesData()
        {
            _employees = TestData.employees; // Получаем сотрудников из TestData
            _MaxFreeId = _employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
        }
        public int Add(Employee employee)
        {
            if(employee is null)
                throw new ArgumentNullException(nameof(employee));

            if(_employees.Contains(employee)) // В БД это делать НЕ НАДО!
                return employee.Id;

            employee.Id = ++_MaxFreeId;
            _employees.Add(employee);
            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if(employee is null)
                return false;

            _employees.Remove(employee);
            return true;
        }

        public bool Edit(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.Contains(employee)) // В БД это делать НЕ НАДО!
                return false;

            var db_employee = GetById(employee.Id);
            
            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;

            //Когда будет БД не забыть вызвать SaveChanges()!!!

            return true;
        }

        public IEnumerable<Employee> GetAll() => _employees;
       

        public Employee? GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }
    }
}
