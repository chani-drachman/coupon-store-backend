using AutoMapper;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{
    public class CouponRequestService: ICouponRequestService
    {
        private readonly ICouponRequestRepository _couponRequestRepository;
        private readonly IMapper _mapper;

        public CouponRequestService(ICouponRequestRepository couponRequestRepository, IMapper mapper)
        {
            _couponRequestRepository = couponRequestRepository;
            _mapper = mapper;
        }

        public async Task<List<CouponRequestDto>> GetAllCouponRequests()
        {
            try
            {
                var request = await _couponRequestRepository.GetAllCouponRequests();
                return _mapper.Map<List<CouponRequestDto>>(request);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving coupon requests", ex);
            }

        }


        public async Task<CouponRequestDto> AddCouponRequest(CouponRequestDto dto)
        {
            try
            {
                var request = new CouponRequest
                {
                    BusinessName = dto.BusinessName,
                    BusinessEmail = dto.BusinessEmail,
                    BusinessPhone = dto.BusinessPhone,
                    CouponTitle = dto.CouponTitle,
                    CouponDescription = dto.CouponDescription,
                    DiscountType = dto.DiscountType,
                    DiscountValue = dto.DiscountValue,
                    ConditionText = dto.ConditionText,
                    ExpirationDate = dto.ExpirationDate,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                var addedRequest= await _couponRequestRepository.AddCouponRequest(request);
                return _mapper.Map<CouponRequestDto>(addedRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the coupon request.", ex);
            }
        }
        public async Task<ProductDto> ApproveCouponRequest(int requestId)
        {
            var request = await _couponRequestRepository.GetCouponRequestById(requestId);
            if (request == null)
                throw new Exception("Coupon request not found");

            if (request.Status != "Pending")
                throw new Exception("Coupon request already processed");

            try
            {
                // מוסיפים עסק וקופון (או רק קופון אם העסק קיים)
                var product = await _couponRequestRepository.AddBusinessAndCoupon(request);

                // מעדכנים את הבקשה ל-Approved
                request.Status = "Approved";
                await _couponRequestRepository.UpdateAsync(request);

                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while approving the coupon request", ex);
            }
        }
    }
}
