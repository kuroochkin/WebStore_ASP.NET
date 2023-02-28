using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient Client) : base(Client, "api/products")
        {
        }

        public Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null)
        {
            var response = Post($"{Address}/new/", new CreateProductDTO
            {
                Name = Name,
                Order = Order,
                Price = Price,
                ImageUrl = ImageUrl,
                Section = Section,
                Brand = Brand,
            });

            var product = response.Content.ReadFromJsonAsync<Product>().Result;
            return product!;
        }

        public bool Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Product product)
        {
            throw new NotImplementedException();
        }

        public Brand? GetBrandById(int? id)
        {
            var brand = Get<Brand>($"{Address}/sections/{id}");
            return brand;
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<Brand>>($"{Address}/brands");
            return brands!;
        }

        public Product? GetProductById(int Id)
        {
            var product = Get<Product>($"{Address}/{Id}");
            return product;
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            var response = Post(Address, Filter ?? new());
            var products = response.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
            return products!;
        }

        public IEnumerable<Section> GetSection()
        {
            var sections = Get<IEnumerable<Section>>($"{Address}/sections");
            return sections!;
        }

        public Section? GetSectionById(int id)
        {
            var section = Get<Section>($"{Address}/sections/{id}");
            return section;
        }
    }
}
