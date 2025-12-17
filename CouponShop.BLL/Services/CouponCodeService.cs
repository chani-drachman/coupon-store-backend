using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Interfaces;
using CouponShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{
    public class CouponCodeService: ICouponCodeService
    {
        private readonly ICouponCodeRepository _couponCodeRepository;

        public CouponCodeService(ICouponCodeRepository couponCodeRepository)
        {
               _couponCodeRepository = couponCodeRepository;
        }
        public async Task<(bool Success, string Message, object? ConsumerInfo)> VerifyCoupon(string code, int businessId)
        {
            var coupon = await _couponCodeRepository.GetCouponCodeByCode(code);
            if (coupon == null) return (false, "קוד הקופון לא קיים.", null);
            if (coupon.IsRedeemed == true) return (false, "קוד הקופון כבר מומש.", null);

            if (coupon.OrderItem.Product.BusinessId != businessId)
                return (false, "אין לך הרשאה לטפל בקופון זה.", null);

            var consumer = coupon.OrderItem.Order.Consumer;
            return (true, "קוד קיים", new { consumer.Name, consumer.Phone, coupon.CouponId });
        }

        public async Task<(bool Success, string Message)> RedeemCouponAsync(int couponId, int businessId)
        {
            var coupon = await _couponCodeRepository.GetCouponCodeById(couponId);
            if (coupon == null) return (false, "קוד הקופון לא קיים.");
            if (coupon.IsRedeemed == true) return (false, "קוד הקופון כבר מומש.");

            if (coupon.OrderItem.Product.BusinessId != businessId)
                return (false, "אין לך הרשאה לטפל בקופון זה.");

            coupon.IsRedeemed = true;
            coupon.RedeemedAt = DateTime.UtcNow;
            await _couponCodeRepository.SaveChangesAsync();
            return (true, "הקופון אומת בהצלחה!");
        }

    }
}
