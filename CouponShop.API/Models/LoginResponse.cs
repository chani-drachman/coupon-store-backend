using CouponShop.DTO;

namespace CouponShop.API.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public ConsumerDto Consumer { get; set; } = null!;
    }
}
