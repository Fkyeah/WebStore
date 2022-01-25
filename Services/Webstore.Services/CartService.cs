using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _cartStore;
        private readonly IProductData _productData;
        
        // Настройка доступа к корзине внутри Cookies
        // При запросе объекта корзины - происходит десереализация из Coocies
        // При установке - сереализация Cookies
        // Считываем Cookies из Request, записываем в Response.

        public CartService(ICartStore cartStore, IProductData productData)
        {
            _cartStore = cartStore;
            _productData = productData;

        }
        public void Add(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = id,
                    Quantity = 1,
                });
            }
            else
            {
                item.Quantity++;
            }

            _cartStore.Cart = cart;
        }

        public void Clear()
        {
            var cart = _cartStore.Cart;

            cart.Items.Clear();

            _cartStore.Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = _cartStore.Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var productsView = products.Products.ToView().ToDictionary(p => p.Id);
            return new CartViewModel
            {
                Items = _cartStore.Cart.Items
                    .Where(item => productsView.ContainsKey(item.ProductId))
                    .Select(item => (productsView[item.ProductId], item.Quantity))
            };
        }

        public void Remove(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }
    }
}
