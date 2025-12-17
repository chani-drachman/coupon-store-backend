using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        //api/order
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                // שליפת ה-ID של המשתמש מתוך ה-Token
                var consumerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (consumerIdClaim == null)
                    return Unauthorized(new { Message = "המשתמש אינו מחובר, אנא התחבר או צור חשבון חדש." });

                var consumerId = int.Parse(consumerIdClaim.Value);
                 var orderDto=_mapper.Map<OrderDto>(orderRequest);
                var order = await _orderService.CreateOrder(orderDto, consumerId);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        //api/order/my-orders
        [Authorize]
        [HttpGet("my-orders")]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
        {
            try
            {
                //שליפת ID מהטוקן של המשתמש
                var consumerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (consumerIdClaim == null)
                    return Unauthorized(new { Message = "המשתמש אינו מחובר, אנא התחבר או צור חשבון חדש." });

                var consumerId = int.Parse(consumerIdClaim.Value);


                var orders = await _orderService.GetOrdersByConsumerId(consumerId);

                if (orders == null || orders.Count == 0)
                    return NotFound(new { Message = "לא נמצאו הזמנות עבור המשתמש." });

                return Ok(orders);

            }
            
                catch (Exception ex)
            {
                return StatusCode(500, new { Message = "שגיאה בשרת: " + ex.Message });
            }
        }
        //api/order/{id}
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                // שליפת ה-ID של המשתמש מתוך הטוקן (למקרה שצריך לבדוק בעלות ההזמנה)
                var consumerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (consumerIdClaim == null)
                    return Unauthorized(new { Message = "המשתמש אינו מחובר, אנא התחבר או צור חשבון חדש." });

                var consumerId = int.Parse(consumerIdClaim.Value);

                var order = await _orderService.GetOrderById(orderId, consumerId);

                if (order == null)
                    return NotFound(new { Message = $"Order with ID {orderId} not found." });

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //api/order
        //יהיה רלוונטי רק כאשר יהיה פאנל ניהול למנהלי האתר בו יוכלו לראות הזמנות, לקוחות וכו
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
        {
            try
            {
                var orders= await _orderService.GetAllOrders();
                if (orders == null) return BadRequest("Error retrieving orders");
                return Ok(orders);

            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }
    }
}
