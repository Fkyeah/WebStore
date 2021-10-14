using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cartModel, OrderViewModel orderModel)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                throw new ArgumentNullException($"Пользователь {userName} не найден!");

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                User = user,
                Phone = orderModel.Phone,
                Address = orderModel.Address,
                Description = orderModel.Description,
            };

            var productIds = cartModel.Items.Select(item => item.product.Id).ToArray();
            var cartProducts = await _db.Products.Where(p => productIds.Contains(p.Id)).ToArrayAsync();

            order.Items = cartModel.Items.Join(
                cartProducts,
                cartItem => cartItem.product.Id,
                cartProduct => cartProduct.Id,
                (cartItem, cartProduct) => new OrderItem
                {
                    Order = order,
                    Product = cartProduct,
                    Price = cartProduct.Price,
                    Quantity = cartItem.quantity,
                }).ToArray();

            await _db.Orders.AddAsync(order);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                .Where(o => o.User.UserName == userName)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return orders;
        }
    }
}
