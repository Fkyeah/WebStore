using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class ProductDetailsControllerTests
    {
        public ProductDetailsControllerTests()
        {
            _productDataMock = new Mock<IProductData>();
            _loggerMock = new Mock<ILogger<ProductDetailsController>>();
        }

        private Mock<IProductData> _productDataMock;
        private Mock<ILogger<ProductDetailsController>> _loggerMock;

        [TestMethod]
        public void Details_Returns_with_Correct_View()
        {
            const int expectedId = 321;
            const string expectedName = "Test product";
            const int expectedOrder = 5;
            const decimal expectedPrice = 13.5m;
            const string expectedImgUrl = "/img/product.img";

            const int expectedBrandid = 7;
            const string expectedBrandName = "Test brand";
            const int expectedBrandOrder = 10;

            const int expectedSectionId = 14;
            const string expectedSectionName = "Test section";
            const int expectedSectionOrder = 123;

            
            _productDataMock
               .Setup(s => s.GetProductById(It.IsAny<int>()))
               .Returns<int>(id => new Product
               {
                   Id = id,
                   Name = expectedName,
                   Order = expectedOrder,
                   Price = expectedPrice,
                   ImageUrl = expectedImgUrl,
                   BrandId = expectedBrandid,
                   Brand = new()
                   {
                       Id = expectedBrandid,
                       Name = expectedBrandName,
                       Order = expectedBrandOrder,
                   },
                   SectionId = expectedSectionId,
                   Section = new()
                   {
                       Id = expectedSectionId,
                       Name = expectedSectionName,
                       Order = expectedSectionOrder,
                   }
               });

            var controller = new ProductDetailsController(_productDataMock.Object, _loggerMock.Object);

            var result = controller.Index(expectedId);

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedImgUrl, model.ImageUrl);
            Assert.Equal(expectedBrandName, model.Brand);
            Assert.Equal(expectedSectionName, model.Section);

            _productDataMock.Verify(s => s.GetProductById(It.Is<int>(id => id > 0)));
            _productDataMock.VerifyNoOtherCalls();
        }
    }
}
