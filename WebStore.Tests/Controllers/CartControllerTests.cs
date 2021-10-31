using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _orderServiceMock = new Mock<IOrderService>();
            _loggerMock = new Mock<ILogger<CartController>>();
        }

        private Mock<ICartService> _cartServiceMock;
        private Mock<IOrderService> _orderServiceMock;
        private Mock<ILogger<CartController>> _loggerMock;

        [TestMethod]
        public async Task CheckOut_ModelState_Invalid_Returns_View_with_Model()
        {
            const string expectedUser = "Test user";
            const string expectedDescription = "Test description";

            
            var controller = new CartController(_cartServiceMock.Object, _loggerMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedUser) }))
                    }
                }
            };

            controller.ModelState.AddModelError("error", "Invalid model");

            var orderModel = new OrderViewModel
            {
                Description = expectedDescription,
            };

            var result = await controller.CheckOut(orderModel, _orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);
            Assert.Equal(expectedDescription, model.Order.Description);

            _cartServiceMock.Verify(s => s.GetViewModel());
            _cartServiceMock.VerifyNoOtherCalls();
            _orderServiceMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task CheckOut_ModelState_Valid_Call_Service_and_Returns_Redirect()
        {
            const string expectedUser = "Test user";

            const string expectedDescription = "Test description";
            const string expectedAddress = "Test address";
            const string expectedPhone = "Test phone";

            _cartServiceMock
               .Setup(c => c.GetViewModel())
               .Returns(new CartViewModel
               {
                   Items = new[] { (new ProductViewModel { Name = "Test product" }, 1) }
               });

            const int expectedOrderId = 1;
            _orderServiceMock
               .Setup(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CartViewModel>(), It.IsAny<OrderViewModel>()))
               .ReturnsAsync(new Order
               {
                   Id = expectedOrderId,
                   Description = expectedDescription,
                   Address = expectedAddress,
                   Phone = expectedPhone,
                   Date = DateTime.Now,
                   Items = Array.Empty<OrderItem>(),
               });

            var controller = new CartController(_cartServiceMock.Object, _loggerMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedUser) }))
                    }
                }
            };

            var orderModel = new OrderViewModel
            {
                Address = expectedAddress,
                Phone = expectedPhone,
                Description = expectedDescription,
            };

            var result = await controller.CheckOut(orderModel, _orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}