namespace CouponShop.API.Models
{
    public class ProductRequest
    {
        public BusinessRequest Business { get; set; } = null!;
        public CouponRequest Product { get; set; } = null!;

    }
}
