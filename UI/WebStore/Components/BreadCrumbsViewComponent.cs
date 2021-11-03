using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbViewModel();
            
            if(int.TryParse(Request.Query["BrandId"], out var brandId))
            {
                model.Brand = _productData.GetBrandById(brandId);
            }    

            if(int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                model.Section = _productData.GetSectionById(sectionId);
                if(model.Section.ParentId is { } parentSectionId)
                {
                    model.Section.Parent = _productData.GetSectionById(parentSectionId);
                }
            }

            if(int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId))
            {
                model.Product = _productData.GetProductById(productId)?.Name;
            }

            return View(model);
        }
    }
}
