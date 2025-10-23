using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderDto> CreateOrder(OrderDto orderDto, int consumerId);
        public Task<List<OrderDto>> GetOrdersByConsumerId(int consumerId);
        public Task<OrderDto?> GetOrderById(int orderId, int consumerId);
        public Task<List<OrderDto>> GetAllOrders();
    }
}
