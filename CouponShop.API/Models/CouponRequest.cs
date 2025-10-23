namespace CouponShop.API.Models
{
    public class CouponRequest
    {
     
        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public string? Details { get; set; }

        public string? ImageUrl { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public bool? IsActive { get; set; }

    }
}
