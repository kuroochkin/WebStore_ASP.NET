using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;
        public IEnumerable<Section> GetSection() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IEnumerable<Product> query = TestData.Products; // Получаем данные из TestData

            if (Filter?.BrandId != null) // Проверяем наличие фильра
                query = query.Where(b => b.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(b => b.SectionId == Filter.SectionId);

            return query;
        }

    }
}
