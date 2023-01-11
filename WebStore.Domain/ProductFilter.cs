using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; set; } // Секция, в которой находится продукт
        public int? BrandId { get; set; } // Бренд продукта
    }
}
