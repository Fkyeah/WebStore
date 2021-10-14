using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
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

        public Section GetSectionById(int id)
        {
            return TestData.Sections.FirstOrDefault(s => s.Id == id);
        }

        public Brand GetBrandById(int id)
        {
            return TestData.Brands.FirstOrDefault(b => b.Id == id);
        }

        public Product GetProductById(int id)
        {
            return TestData.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
