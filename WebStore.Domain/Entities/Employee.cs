using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Employee : Entity
    {
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }
    }
}
