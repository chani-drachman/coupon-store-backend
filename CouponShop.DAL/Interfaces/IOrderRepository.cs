using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> CreateOrder(Order order);
        public Task<List<Order>> GetOrdersByConsumerId(int consumerId);
        public Task<Order?> GetOrderById(int orderId);
        public Task<List<Order>> GetAllOrders();


    }
}
