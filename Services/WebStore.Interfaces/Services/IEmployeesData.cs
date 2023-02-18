using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    // Что должен уметь делать сервис управления сотрудниками?
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll(); // Вернуть всех сотрудников
        Employee? GetById(int id); // Вернуть сотрудника по ID
        bool Edit(Employee employee); // Редактировать сотрудника
        bool Delete(int id); // Удаление сотрудника
        int Add(Employee employee); // Добавление сотрудника
    }
}
