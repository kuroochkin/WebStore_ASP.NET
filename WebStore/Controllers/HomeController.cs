using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var products = ProductData.GetProducts()
                .OrderBy(p => p.Order)
                .Take(6)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                });
            ViewBag.Products = products;

            return View(); // Первое представление
        }

        public IActionResult Contacts() => View();
        
    }
}
