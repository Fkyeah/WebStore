using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    public class SiteMapController : ControllerBase
    {
        private readonly IProductData _productData;

        public SiteMapController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index()
        {
            List<SitemapNode> nodes = new List<SitemapNode>
            {
                new(Url.Action("Index", "Home")),
                new(Url.Action("Index", "Blog")),
                new(Url.Action("Index", "BlogSimple")),
                new(Url.Action("Index", "ContactUs")),
                new(Url.Action("Index", "Shop")),
            };

            nodes.AddRange(
                _productData
                    .GetSections()
                    .Select(s => new SitemapNode(
                                Url.Action("Index", "Shop", new 
                                { 
                                    SectionId = s.Id 
                                }))));

            nodes.AddRange(
                _productData
                    .GetBrands()
                    .Select(s => new SitemapNode(
                                Url.Action("Index", "Shop", new
                                {
                                    BrandId = s.Id
                                }))));

            nodes.AddRange(
                _productData
                    .GetProducts()
                    .Products
                    .Select(s => new SitemapNode(
                                Url.Action("Index", "ProductDetails", new { s.Id }))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
