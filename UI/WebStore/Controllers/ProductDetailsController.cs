using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly IProductData _products;
        private readonly ILogger<ProductDetailsController> _logger;

        public ProductDetailsController(IProductData products, ILogger<ProductDetailsController> logger)
        {
            _products = products;
            _logger = logger;
        }
        public IActionResult Index(int id)
        {
            var product = _products.GetProductById(id);
            _logger.LogInformation("Получение товара с ID = {0}", id);
            return View(product.ToView());
        }
    }
}
