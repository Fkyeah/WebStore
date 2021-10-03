using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }
        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands;
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> products = _db.Products;
            if (filter?.SectionId != null)
                products = products.Where(el => el.SectionId == filter.SectionId);
            if (filter?.BrandId != null)
                products = products.Where(el => el.BrandId == filter.BrandId);
            return products;
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }
    }
}
