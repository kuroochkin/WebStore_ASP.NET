using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<ProductsController> _Logger;

        public ProductsController(IProductData ProductData, ILogger<ProductsController> Logger)
        {
            this._ProductData = ProductData;
            this._Logger = Logger;
        }

        public IProductData ProductData { get; }

        public IActionResult Index()
        {
            var products = _ProductData.GetProducts();
            return View(products);
        }
    }
}
