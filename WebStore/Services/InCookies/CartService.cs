using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class CartService : ICartService
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly string _cartName;

        // Настройка доступа к корзине внутри Cookies
        // При запросе объекта корзины - происходит десереализация из Coocies
        // При установке - сереализация Cookies
        // Считываем Cookies из Request, записываем в Response.
        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;

                var cart_cookies = context.Request.Cookies[_cartName];
                // если корзины не было
                if (cart_cookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                // если корзина была, ее необходимо подменить новой
                ReplaceCart(cookies, cart_cookies);
                return JsonConvert.DeserializeObject<Cart>(cart_cookies);
            }
            set
            {
                ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cart);
        }
        public CartService(HttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = httpContextAccessor;
            _productData = productData;

            var user = httpContextAccessor.HttpContext.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;
            _cartName = $"Webstore.Cart{userName}";
        }
        public void Add(int id)
        {
            var cart = Cart;

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

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            
            if (item is null)
                return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            return null;
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            Cart = cart;
        }
    }
}
