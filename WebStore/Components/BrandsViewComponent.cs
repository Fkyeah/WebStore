using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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
