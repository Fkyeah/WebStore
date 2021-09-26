using System.Collections.Generic;
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
    }
}
