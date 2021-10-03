using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Model;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections()
        {
            return TestData.Sections;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return TestData.Brands;
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IEnumerable<Product> products = TestData.Products;
            if (filter?.SectionId != null)
                products = products.Where(el => el.SectionId == filter.SectionId);
            if (filter?.BrandId != null)
                products = products.Where(el => el.BrandId == filter.BrandId);
            return products;
        }
    }
}
