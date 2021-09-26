using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductData _products;

        public ShopController(IProductData products)
        {
            _products = products;
        }
        public IActionResult Index(int? brandId, int? sectionId)
        {
            var products = _products.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId });
            var model = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.Select(el => new ProductViewModel
                {
                    Id = el.Id,
                    Name = el.Name,
                    Price = el.Price,
                    ImageUrl = el.ImageUrl,
                    Order = el.Order
                }).OrderBy(p => p.Order).ToList()

            };
            return View(model);
        }
    }
}
