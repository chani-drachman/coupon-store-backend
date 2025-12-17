using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponRequestsController : ControllerBase
    {
        private readonly ICouponRequestService _couponRequestService;
        private readonly IMapper _mapper;

        public CouponRequestsController(ICouponRequestService couponRequestService, IMapper mapper)
        {
            _couponRequestService = couponRequestService;
            _mapper = mapper;
        }
        //api/couponrequest
        [HttpGet]
        public async Task<List<CouponRequestDto>> GetAllCouponRequests()
        {
            try
            {
                return await _couponRequestService.GetAllCouponRequests();
            }
            catch (Exception ex)
            { throw new Exception("An error occured while retrieving coupon requests", ex); }
        }

        [HttpPost]
        public async Task<ActionResult<CouponRequestDto>> AddCouponRequest([FromBody] AddCouponRequest request)
        {
            try
            {
               var requestDetails= _mapper.Map<AddCouponRequestDto>(request);
                var result= await _couponRequestService.AddCouponRequest(requestDetails);
                return Ok(result);
            }
            catch (Exception ex)
            { return StatusCode(500, new { Message = "Failed to add coupon request.", Detail = ex.Message }); }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/approve")]
        public async Task<ActionResult> ApproveCouponRequest(int id)
        {
            try
            {
                var product = await _couponRequestService.ApproveCouponRequest(id);
                return Ok(new
                {
                    Message = "Coupon request approved",
                    ProductId = product.ProductId,
                         ProductName = product.Description
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("המייל כבר קיים במערכת"))
                {
                    return BadRequest(new { error = ex.Message });
                }
                return StatusCode(500, new { error = "שגיאה פנימית בשרת", details = ex.Message });
            }
        }
    }
}
