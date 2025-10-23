namespace CouponShop.API.Models
{
    public class UpdateConsumerRequest
    {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
