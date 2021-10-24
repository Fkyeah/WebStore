using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(WebStore.Interfaces.WebAPI.Products)]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var result = _productData.GetSections();
            return Ok(result.ToDTO());
        }

        [HttpGet("sections/{id}")]
        public IActionResult GetSectionById(int id)
        {
            var result = _productData.GetSectionById(id);
            if (result is null)
                return NotFound();
            return Ok(result.ToDTO());
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var result = _productData.GetBrands();
            return Ok(result.ToDTO());
        }

        [HttpGet("brands/{id}")]
        public IActionResult GetBrandById(int id)
        {
            var result = _productData.GetBrandById(id);
            if (result is null)
                return NotFound();
            return Ok(result.ToDTO());
        }

        [HttpPost]
        public IActionResult GetProducts([FromBody] ProductFilter filter = null)
        {
            var result = _productData.GetProducts(filter);
            return Ok(result.ToDTO());
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var result = _productData.GetProductById(id);
            if (result is null)
                return NotFound();
            return Ok(result.ToDTO());
        }
    }
}
