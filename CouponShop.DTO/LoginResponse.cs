using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DTO
{
    public class LoginResponse
    {
        public ConsumerDto Consumer { get; set; } = null!;
        public string token { get; set; } = null!;
    }
}
