using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
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
        public IViewComponentResult Invoke(string brandRowId)
        {
            var brandId = int.TryParse(brandRowId, out int id) ? id : (int?)null;

            var brands = GetBrands();

            return View(new SelectableBrandsViewModel
            { 
                Brands = brands,
                BrandId = brandId,
            });
        }

        private IEnumerable<BrandViewModel> GetBrands()
        {
            return _productData
                .GetBrands()
                .OrderByDescending(el => el.Order)
                .Select(el => new BrandViewModel
                {
                    Id = el.Id,
                    Name = el.Name,
                    Order = el.Order,
                })
                .ToList();
        }
    }
}
