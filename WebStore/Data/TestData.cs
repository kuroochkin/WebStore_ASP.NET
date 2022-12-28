using WebStore.Models;

namespace WebStore.Data
{
    public class TestData
    {
        public static List<Employee> employees { get; } = new()
        {
            new Employee {Id = 1, LastName = "Келин", FirstName = "Кирилл", Patronymic = "Вячеславович", Age = 19},
            new Employee {Id = 2, LastName = "Горбатых", FirstName = "Александр", Patronymic = "Андреевич", Age = 18},
            new Employee {Id = 3, LastName = "Ярочевский", FirstName = "Максим", Patronymic = "Павлович", Age = 18}
        };
    }
}
