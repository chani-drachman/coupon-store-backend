namespace CouponShop.API.Models
{
    public class OrderRequest
    {
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
