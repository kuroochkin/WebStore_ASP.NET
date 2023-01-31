using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities.Orders
{
    public class Order : Entity
    {
        [Required]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Address { get; set; } = null!;
        
        public string? Description { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [NotMapped] // НЕ БУДЕТ В БД
        public decimal TotalPrice => Items.Sum(item => item.TotalItemsPrice);
    }
}
