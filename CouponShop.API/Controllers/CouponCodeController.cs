using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponCodeController : ControllerBase
    {
        private readonly ICouponCodeService _couponService;
        public CouponCodeController(ICouponCodeService couponService) => _couponService = couponService;

        [Authorize(Roles = "Business")]
        [HttpPost("verify-coupon")]
        public async Task<IActionResult> VerifyCoupon([FromBody] VerifyCouponRequest request)
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")!.Value);
            var result = await _couponService.VerifyCoupon(request.Code, businessId);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.ConsumerInfo);
        }

        [Authorize(Roles = "Business")]
        [HttpPost("redeem-coupon/{couponId}")]
        public async Task<IActionResult> RedeemCoupon(int couponId)
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")!.Value);
            var result = await _couponService.RedeemCouponAsync(couponId, businessId);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
