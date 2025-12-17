namespace CouponShop.API.Models
{
    public class ConsumerRequest
    {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } 
        public string Role { get; set; } = "User";
    }
}
