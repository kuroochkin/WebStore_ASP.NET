using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Services.Interfaces;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : Controller
    {
        public readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        [HttpGet("sections")]
        public IActionResult GetSection()
        {
            var section = _ProductData.GetSection();
            return Ok(section);
        }

        [HttpGet("sections/{Id}")]
        public IActionResult GetSectionById(int Id)
        {
            var section = _ProductData.GetSectionById(Id);
            if (section is null)
                return NotFound();
            return Ok(section);
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _ProductData.GetBrands();
            return Ok(brands);
        }

        [HttpGet("brands/{Id}")]
        public IActionResult GetBrandById(int Id)
        {
            var brand = _ProductData.GetBrandById(Id);
            if (brand is null)
                return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter? Filter = null)
        {
            var products = _ProductData.GetProducts(Filter);
            return Ok(products);
        }

        [HttpGet("{Id}")]
        public IActionResult GetProductGyId(int Id)
        {
            var product = _ProductData.GetProductById(Id);
            if (product is null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost("new/{Name}")]
        public IActionResult CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null)
        {
            var product = _ProductData.CreateProduct(Name, Order, Price, ImageUrl, Brand);
            return CreatedAtAction(nameof(GetProductGyId), new { Id = product.Id }, product);
        }
    }
}
