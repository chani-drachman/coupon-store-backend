using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DTO
{
    public class CouponCodeDto
    {
        public int CouponId { get; set; }


        public string Code { get; set; } = null!;

        public bool? IsRedeemed { get; set; }

        public DateTime? RedeemedAt { get; set; }

      
    }
}
