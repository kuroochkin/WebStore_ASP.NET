using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int SectionId { get; set; }

        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }

        public int? BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        public string ImageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
