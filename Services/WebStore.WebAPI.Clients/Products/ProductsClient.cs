using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient httpClient) : base(httpClient, WebStore.Interfaces.WebAPI.Products)
        {
        }

        public Brand GetBrandById(int id)
        {
            var result = Get<BrandDTO>($"{_controllerAddress}/brands/{id}");
            return result.FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            var result = Get<IEnumerable<BrandDTO>>($"{_controllerAddress}/brands");
            return result.FromDTO();
        }

        public Product GetProductById(int id)
        {
            var result = Get<ProductDTO>($"{_controllerAddress}/{id}");
            return result.FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var response = Post(_controllerAddress, filter ?? new());
            var productDTOs = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
            return productDTOs.FromDTO();

        }

        public Section GetSectionById(int id)
        {
            var result = Get<SectionDTO>($"{_controllerAddress}/sections/{id}");
            return result.FromDTO();
        }

        public IEnumerable<Section> GetSections()
        {
            var result = Get<IEnumerable<SectionDTO>>($"{_controllerAddress}/sections");
            return result.FromDTO();
        }
    }
}
