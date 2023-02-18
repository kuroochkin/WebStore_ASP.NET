using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна!")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Некорректный ввод")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно!")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Некорректный ввод")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Некорректный ввод")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        [Range(18,80, ErrorMessage = "Возраст от 18 до 80")]
        public int Age { get; set; }
    }
}
