using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        public WebStoreDB _db { get; }
        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (Filter?.SectionId is { } section_id) // Накладываем на запрос фильтр
                query = query.Where(p => p.SectionId == section_id);

            if (Filter?.BrandId is { } brand_id)
                query = query.Where(p => p.BrandId == brand_id);

            return query;
        }

        public IEnumerable<Section> GetSection() => _db.Sections;
        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public Product? GetProductById(int Id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == Id);

    }
}
