using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Mapping;
using WebStore.Interfaces.Services;

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
                Products = products.Select(el => el.ToView())
                    .OrderBy(p => p.Order).ToList()

            };
            return View(model);
        }
    }
}
