using System.ComponentModel.DataAnnotations;

namespace CouponShop.API.Models
{
    public class BusinessRequest
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
  

    }
}
