using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient httpClient) : base(httpClient, "api/products")
        {
        }

        public Brand GetBrandById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Brand> GetBrands()
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public Section GetSectionById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Section> GetSections()
        {
            throw new NotImplementedException();
        }
    }
}
