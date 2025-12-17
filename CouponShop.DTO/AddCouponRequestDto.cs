using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DTO
{
    public class AddCouponRequestDto
    {
        public string PrivateName { get; set; }
        public string BusinessName { get; set; }
        public int CategoryId { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessAddress { get; set; }
        public string CouponTitle { get; set; }
        public string? CouponDescription { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public string? ConditionText { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
