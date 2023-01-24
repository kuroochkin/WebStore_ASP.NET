﻿using WebStore.Domain.Entities;
using Employee = WebStore.Domain.Entities.Employee;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> employees { get; } = new()
        {
            new Employee {LastName = "Келин", FirstName = "Кирилл", Patronymic = "Вячеславович", Age = 19},
            new Employee {LastName = "Горбатых", FirstName = "Александр", Patronymic = "Андреевич", Age = 18},
            new Employee {LastName = "Ярочевский", FirstName = "Максим", Patronymic = "Павлович", Age = 18}
        };

        public static IEnumerable<Section> Sections { get; } = new[]
        {
              new Section { Id = 1, Name = "Игровые консоли", Order = 0 },
              new Section { Id = 2, Name = "Приставки", Order = 0, ParentId = 1 },
              new Section { Id = 3, Name = "Геймпады", Order = 1, ParentId = 1 },
              new Section { Id = 4, Name = "Видеоигры", Order = 2, ParentId = 1 },
              new Section { Id = 5, Name = "Техника Apple", Order = 1 },
              new Section { Id = 6, Name = "MacBook", Order = 0, ParentId = 5 },
              new Section { Id = 7, Name = "iPad", Order = 1, ParentId = 5 },
              new Section { Id = 8, Name = "iPhone", Order = 2, ParentId = 5 },
              new Section { Id = 9, Name = "Watch", Order = 3, ParentId = 5 },
              //new Section { Id = 18, Name = "Для женщин", Order = 2 },
              //new Section { Id = 19, Name = "Fendi2", Order = 0, ParentId = 18 },
              //new Section { Id = 20, Name = "Guess2", Order = 1, ParentId = 18 },
              //new Section { Id = 21, Name = "Valentino2", Order = 2, ParentId = 18 },
              //new Section { Id = 22, Name = "Dior", Order = 3, ParentId = 18 },
              //new Section { Id = 23, Name = "Versace", Order = 4, ParentId = 18 },
              //new Section { Id = 24, Name = "Для детей", Order = 3 },
              //new Section { Id = 25, Name = "Мода", Order = 4 },
              //new Section { Id = 26, Name = "Для дома", Order = 5 },
              //new Section { Id = 27, Name = "Интерьер", Order = 6 },
              //new Section { Id = 28, Name = "Одежда", Order = 7 },
              //new Section { Id = 29, Name = "Сумки", Order = 8 },
              //new Section { Id = 30, Name = "Обувь", Order = 9 },
        };

        public static IEnumerable<Brand> Brands { get; } = new[]
        {
            new Brand { Id = 1, Name = "Sony", Order = 0 },
            new Brand { Id = 2, Name = "Apple", Order = 1 },
            new Brand { Id = 3, Name = "Microsoft", Order = 2 },
            //new Brand { Id = 4, Name = "Ronhill", Order = 3 },
            //new Brand { Id = 5, Name = "Oddmolly", Order = 4 },
            //new Brand { Id = 6, Name = "Boudestijn", Order = 5 },
            //new Brand { Id = 7, Name = "Rosch creative culture", Order = 6 },
        };

        public static IEnumerable<Product> Products { get; } = new[]
        {
            //Картинки лежат в images/shop
            new Product { Id = 1, Name = "Sony PlayStation 5 + Руль Logitech G29 Driving Force и педали + Игра  F1 2021", Price = 95448, ImageUrl = "ps5.jpg", Order = 0, SectionId = 2, BrandId = 1 },
           
            


        };
    }
}
