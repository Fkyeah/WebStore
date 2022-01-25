﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        public CartServiceTests()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new() { ProductId = 1, Quantity = 1 },
                    new() { ProductId = 2, Quantity = 3 },
                }
            };

            _cartViewModel = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
                }
            };

            var products = new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Price = 1.1m,
                    Order = 1,
                    ImageUrl = "img_1.png",
                    Brand = new Brand { Id = 1, Name = "Brand 1", Order = 1 },
                    SectionId = 1,
                    Section = new Section { Id = 1, Name = "Section 1", Order = 1 },
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Price = 2.2m,
                    Order = 2,
                    ImageUrl = "img_2.png",
                    Brand = new Brand { Id = 2, Name = "Brand 2", Order = 2 },
                    SectionId = 2,
                    Section = new Section { Id = 2, Name = "Section 2", Order = 2 },
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    Price = 3.3m,
                    Order = 3,
                    ImageUrl = "img_3.png",
                    Brand = new Brand { Id = 3, Name = "Brand 3", Order = 3 },
                    SectionId = 3,
                    Section = new Section { Id = 3, Name = "Section 3", Order = 3 },
                },
            };

            _productDataMock = new Mock<IProductData>();
            _productDataMock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(new ProductsPage(products, products.Length));

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);

            _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object);
        }

        private Cart _cart;
        private CartViewModel _cartViewModel;
        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;
        private ICartService _cartService;

        [TestMethod]
        public void Cart_Class_ItemsCount_returns_Correct_Quantity()
        {
            var cart = _cart;
            var expectedItemsCount = cart.Items.Sum(i => i.Quantity);

            var actualItemsCount = cart.ItemsCount;

            Assert.Equal(expectedItemsCount, actualItemsCount);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var expectedItemsCount = _cartViewModel.Items.Sum(i => i.quantity);

            var actualItemsCount = _cartViewModel.ItemsCount;

            Assert.Equal(expectedItemsCount, actualItemsCount);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_TotalPrice()
        {
            var expected_total_price = _cartViewModel.Items.Sum(item => item.quantity * item.product.Price);

            var actual_total_price = _cartViewModel.TotalSum;

            Assert.Equal(expected_total_price, actual_total_price);
        }

        [TestMethod]
        public void CartService_Add_WorkCorrect()
        {
            _cart.Items.Clear();
            const int expectedId = 5;
            const int expectedItemsCount = 1;

            _cartService.Add(expectedId);
            var actualItemsCount = _cart.ItemsCount;

            Assert.Equal(expectedItemsCount, actualItemsCount);
            Assert.Single(_cart.Items);
            Assert.Equal(expectedId, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartService_Remove_Correct_Item()
        {
            const int itemId = 1;
            const int expectedProductId = 2;

            _cartService.Remove(itemId);

            Assert.Single(_cart.Items);
            Assert.Equal(expectedProductId, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartService_Clear_ClearCart()
        {
            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int itemId = 2;

            const int expectedQuantity = 2;
            const int expectesItemsCount = 3;
            const int expectedProductsCount = 2;

            _cartService.Decrement(itemId);
            var items = _cart.Items.ToArray();

            Assert.Equal(expectesItemsCount, _cart.ItemsCount);
            Assert.Equal(expectedProductsCount, _cart.Items.Count);            
            Assert.Equal(itemId, items[1].ProductId);
            Assert.Equal(expectedQuantity, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int itemId = 1;
            const int expectedItemsCount = 3;

            _cartService.Decrement(itemId);

            Assert.Equal(expectedItemsCount, _cart.ItemsCount);
            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartService_GetViewModel_WorkCorrect()
        {
            const int expectedItemsCount = 4;
            const decimal expectedFirstProductPrice = 1.1m;

            var result = _cartService.GetViewModel();

            Assert.Equal(expectedItemsCount, result.ItemsCount);
            Assert.Equal(expectedFirstProductPrice, result.Items.First().product.Price);
        }
    }
}