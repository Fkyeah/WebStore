using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BrandsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var brands = _productData.GetBrands();
            var brandsViews = brands
                .OrderByDescending(el => el.Order)
                .Select(el => new BrandViewModel
                {
                    Id = el.Id,
                    Name = el.Name,
                    Order = el.Order,
                })
                .ToList();
            return View(brandsViews);
        }
    }
}
