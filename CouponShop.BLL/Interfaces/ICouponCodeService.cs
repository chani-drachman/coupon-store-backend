using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface ICouponCodeService
    {
        public Task<(bool Success, string Message, object? ConsumerInfo)> VerifyCoupon(string code, int businessId);
        public Task<(bool Success, string Message)> RedeemCouponAsync(int couponId, int businessId);
    }
}
