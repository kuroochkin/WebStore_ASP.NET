using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; } // Родительская секция(может и не быть)

        [ForeignKey(nameof(ParentId))]
        public Section Parent { get; set; } // Навигационное свойство

        public ICollection<Product> Products { get; set; }
    }
}

