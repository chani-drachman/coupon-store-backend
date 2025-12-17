using System.ComponentModel.DataAnnotations;

namespace CouponShop.API.Models
{
    public class AddCouponRequest
    {
        [Required]
        public string PrivateName { get; set; } = null!;
        [Required]
        public string BusinessName { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; } 
        public string BusinessEmail { get; set; } = null!;
        public string BusinessPhone { get; set; } = null!;
        public string BusinessAddress { get; set; } = null!;
        public string CouponTitle { get; set; } = null!;
        public string? CouponDescription { get; set; }
        public string DiscountType { get; set; } = null!;
        public decimal? DiscountValue { get; set; }
        public string? ConditionText { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
