using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebStore.Infrastructure.Mapping;
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
                .ToView();
                
            ViewBag.Products = products;

            return View(); // Первое представление
        }

        public IActionResult Contacts() => View();
        
    }
}
