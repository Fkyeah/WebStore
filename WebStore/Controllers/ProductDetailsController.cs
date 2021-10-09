using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly IProductData _products;

        public ProductDetailsController(IProductData products)
        {
            _products = products;
        }
        public IActionResult Index(int id)
        {
            var product = _products.GetProductById(id);
            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand?.Name,
                Section = product.Section.Name,
            });
        }
    }
}
