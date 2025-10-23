using AutoMapper;
using Azure.Core;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<OrderDto> CreateOrder(OrderDto orderDto, int consumerId)
        {
            var order = new Order
            {
                ConsumerId = consumerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };
            foreach (var item in orderDto.Items)
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = 20, // מחיר קבוע 20
                    CouponCodes = new List<CouponCode>()
                };

                // יצירת קודי קופון
                for (int i = 0; i < item.Quantity; i++)
                {
                    orderItem.CouponCodes.Add(new CouponCode
                    {
                        Code = Guid.NewGuid().ToString("N")[..8], // קוד ייחודי קצר
                        IsRedeemed = false
                    });
                }

                order.OrderItems.Add(orderItem);
            }

            // חישוב סכום כולל
            order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

          

             var addedOrder= await _orderRepository.CreateOrder(order);
            return _mapper.Map<OrderDto>(addedOrder);

        }

        public async Task<List<OrderDto>> GetOrdersByConsumerId(int consumerId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByConsumerId(consumerId);
                return _mapper.Map<List<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving orders for consumer with ID {consumerId}.", ex);
            }
        }
        public async Task<OrderDto?> GetOrderById(int orderId, int consumerId)
        {
            try
            {
                // שליפת ההזמנה מה-Repositoy
                var order = await _orderRepository.GetOrderById(orderId);

                if (order == null) return null;

                // בדיקה אם ההזמנה שייכת למשתמש (אם רוצים להגביל גישה)
                if (order.ConsumerId != consumerId)
                    throw new UnauthorizedAccessException("you don´t have an access.");

                // המרה ל-DTO כולל פרטי OrderItems וקודי קופון
                var orderDto = _mapper.Map<OrderDto>(order);
                return orderDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving order {orderId}.", ex);
            }
        }


        public async Task<List<OrderDto>> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrders();
                return _mapper.Map<List<OrderDto>>(orders);

            }
            catch (Exception ex) { throw new Exception("An error occured while retrieving orders", ex); }
        }
    }
}
