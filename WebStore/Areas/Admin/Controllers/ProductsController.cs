using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebStore.Areas.Admin.ViewModels;
using WebStore.DAL.Migrations;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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


        public IActionResult Index()
        {
            var products = _ProductData.GetProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            var model = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                SectionId = product.SectionId,
                Section = product.Section.Name,
                Brand = product.Brand?.Name,
                BrandId = product.BrandId,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            };

            return View(model); // Отправка модели на обработку
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var product = _ProductData.GetProductById(Model.Id);

            if (product is null)
                return NotFound();

            var brand = _ProductData.GetBrandById(Model.BrandId ?? -1);
            var section = _ProductData.GetSectionById(Model.SectionId);
            //Копируем данные
            product.Name = Model.Name;
            product.ImageUrl = Model.ImageUrl;
            product.Price = Model.Price;
            product.Order = Model.Order;
            product.Brand = brand;
            product.Section = section;

            _ProductData.Edit(product);


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            if (Id < 0)
                return BadRequest();

            var product = _ProductData.GetProductById(Id);
            if (product is null)
                return NotFound();

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Section = product.Section.Name,
                Brand = product.Brand?.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int Id)
        {
            var product = _ProductData.GetProductById(Id);
            if (product is null)
                return NotFound();

            _ProductData.Delete(Id);

            return RedirectToAction("Index", "Products");
        }
    }
    
}
