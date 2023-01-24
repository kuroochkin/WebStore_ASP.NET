using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        public CartController(ICartService CartService) => _CartService = CartService;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(int Id)
        {
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int Id)
        {
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int Id)
        {
            return RedirectToAction("Index", "Cart");
        }
    }
}
