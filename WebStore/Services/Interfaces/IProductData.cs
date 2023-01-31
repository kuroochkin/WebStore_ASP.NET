using WebStore.Domain;
using WebStore.Domain.Entities;

// Что умеет наш сервис?

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSection();

        Section? GetSectionById(int id);
        
        IEnumerable<Brand> GetBrands();

        Brand? GetBrandById(int? id);
        IEnumerable<Product> GetProducts(ProductFilter? Filter = null); // Можно и не указывать фильтр в параметрах

        Product GetProductById(int Id); // поиск товара по Id

        Product CreateProduct(string Name, string Order, decimal Price, string ImageIrl, string Section, string? Brand = null);
        bool Edit(Product product);
        bool Delete(int Id);
    }
}
