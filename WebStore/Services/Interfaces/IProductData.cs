using WebStore.Domain;
using WebStore.Domain.Entities;

// Что умеет наш сервис?

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSection();
        IEnumerable<Brand> GetBrands();
        IEnumerable<Product> GetProducts(ProductFilter? Filter = null); // Можно и не указывать фильтр в параметрах

    }
}
