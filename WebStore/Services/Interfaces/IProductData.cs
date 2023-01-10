using WebStore.Domain.Entities;

// Что умеет наш сервис?

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSection();
        IEnumerable<Brand> GetBrands();
    }
}
