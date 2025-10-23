using CouponShop.DAL.Context;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CouponShopContext _context;

        public OrderRepository(CouponShopContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateException ex)
            {
                // טיפול בשגיאות מסד נתונים ספציפיות
                throw new Exception("An error occurred while adding the order to the database.", ex);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while adding the order.", ex);
            }
        }
        public async Task<List<Order>> GetOrdersByConsumerId(int consumerId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.CouponCodes)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Business) 
                    .Where(o => o.ConsumerId == consumerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving orders.", ex);
            }
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.CouponCodes)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Business) 
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order {orderId} from database.", ex);
            }
        }




        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.Consumer)
                    .Include(o => o.OrderItems).
                    ThenInclude(oi => oi.Product).ThenInclude(p => p.Business)
                    .ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while retrieving orders.", ex);
            }
        }
    
    }
}
