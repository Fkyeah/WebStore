using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductData _products;
        private readonly IConfiguration _configuration;

        public ShopController(IProductData products, IConfiguration configuration)
        {
            _products = products;
            _configuration = configuration;
        }
        public IActionResult Index(int? brandId, int? sectionId, int page = 1, int? pageSize = null)
        {
            var currentPageSize = pageSize ?? (int.TryParse(_configuration["CatalogPageSize"].ToString(), out var size) ? size : (int?)null);
            var products = _products.GetProducts(new ProductFilter 
            { 
                BrandId = brandId, 
                SectionId = sectionId, 
                PageNumber = page,
                PageSize = currentPageSize,
            });
            var model = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.Products.Select(el => el.ToView())
                    .OrderBy(p => p.Order).ToList()

            };
            return View(model);
        }
    }
}
