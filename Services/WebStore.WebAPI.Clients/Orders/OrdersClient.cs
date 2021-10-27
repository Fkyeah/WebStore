using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient Client) : base(Client, WebStore.Interfaces.WebAPI.Orders) { }
        
        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{_controllerAddress}/user/{userName}").ConfigureAwait(false);
            return orders.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await GetAsync<OrderDTO>($"{_controllerAddress}/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cartModel, OrderViewModel orderModel)
        {
            var model = new CreateOrderDTO
            {
                Items = cartModel.ToDTO(),
                Order = orderModel,
            };

            var response = await PostAsync($"{_controllerAddress}/{userName}", model).ConfigureAwait(false);
            var order = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<OrderDTO>()
               .ConfigureAwait(false);
            return order.FromDTO();
        }
    }
}
