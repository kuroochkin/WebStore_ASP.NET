using System.Runtime.CompilerServices;
using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel? ToView(this Product product) => product is null
            ? null
            : new ProductViewModel // создаем из товара его ViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };

        public static Product? FromView(this ProductViewModel product) => product is null
            ? null
            : new Product // cоздаем из ViewModel - ТОВАР
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };

        public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product?> products) => products.Select(ToView);

        public static IEnumerable<Product?> ToView(this IEnumerable<ProductViewModel?> products) => products.Select(FromView);

    }
}
