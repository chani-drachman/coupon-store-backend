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
        public async Task<CouponRequestDto> AddCouponRequest(AddCouponRequest request)
        {
            try
            {
               var requestDetails= _mapper.Map<CouponRequestDto>(request);
                return await _couponRequestService.AddCouponRequest(requestDetails);
            }
            catch (Exception ex)
            { throw new Exception("failed to add coupon request", ex); }
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
