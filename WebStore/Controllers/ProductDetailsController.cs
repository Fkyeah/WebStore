using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

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
            return View();
        }
    }
}
