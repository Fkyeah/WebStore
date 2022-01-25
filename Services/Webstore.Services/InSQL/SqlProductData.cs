using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public Brand GetBrandById(int id)
        {
            return _db.Brands.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands;
        }

        public Product GetProductById(int id)
        {
            return _db.Products
                .Include(b => b.Brand)
                .Include(s => s.Section)
                .FirstOrDefault(p => p.Id == id);
        }

        public ProductsPage GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> products = _db.Products
                .Include(b => b.Brand)
                .Include(s => s.Section);

            if (filter?.Ids?.Length > 0)
                products = products.Where(product => filter.Ids.Contains(product.Id));
            else
            {
                if (filter?.SectionId != null)
                    products = products.Where(el => el.SectionId == filter.SectionId);
                if (filter?.BrandId != null)
                    products = products.Where(el => el.BrandId == filter.BrandId);

            }

            var totalCount = products.Count();

            if (filter.PageSize > 0 && filter.PageNumber > 0)
                products = products
                   .OrderBy(v => v.Order)
                   .Skip((int)((filter.PageNumber - 1) * filter.PageSize))
                   .Take((int)filter.PageSize);

            return new ProductsPage(products.AsEnumerable(), totalCount);
        }

        public Section GetSectionById(int id)
        {
            return _db.Sections.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }
    }
}
