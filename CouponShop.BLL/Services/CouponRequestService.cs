using AutoMapper;
using CouponShop.BLL.Infrastructures;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DTO;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{
    public class CouponRequestService: ICouponRequestService
    {
        private readonly ICouponRequestRepository _couponRequestRepository;
        private readonly IConsumerRepository _consumerRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public CouponRequestService(ICouponRequestRepository couponRequestRepository,IConsumerRepository consumerRepository,
            IEmailService emailService, IProductRepository productRepository, IBusinessRepository businessRepository, IMapper mapper)
        {
            _couponRequestRepository = couponRequestRepository;
            _consumerRepository = consumerRepository;
            _businessRepository = businessRepository;
            _productRepository = productRepository;
            _emailService = emailService;
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


        public async Task<CouponRequestDto> AddCouponRequest(AddCouponRequestDto dto)
        {
            try
            {
                var request = new CouponRequest
                {
                    PrivateName = dto.PrivateName,
                    BusinessName = dto.BusinessName,
                    BusinessEmail = dto.BusinessEmail,
                    BusinessPhone = dto.BusinessPhone,
                    BusinessAddress = dto.BusinessAddress,
                    CategoryId = dto.CategoryId,
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
            var existingConsumer = await _consumerRepository.GetConsumerByEmail(request.BusinessEmail);
            if (existingConsumer != null)
            {
                throw new Exception("המייל כבר קיים במערכת");
            }


            try
            {

                var consumer = new Consumer
                {
                    Name = request.PrivateName,
                    Phone = request.BusinessPhone,
                    Address = request.BusinessAddress,
                    Email = request.BusinessEmail,
                    CreatedAt = DateTime.Now,
                    Role = "Business"
                };
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                consumer.ResetPasswordToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
                consumer.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);
                var encodedToken = Uri.EscapeDataString(token);

                await _consumerRepository.AddConsumer(consumer);

                var business = new Business
                {
                    ConsumerId = consumer.ConsumerId,
                    Name = request.BusinessName,
                    Phone = request.BusinessPhone,
                    Address = request.BusinessAddress,
                    Email = request.BusinessEmail,

                };
                await _businessRepository.AddBusiness(business);


                var coupon = new Product
                {
                    CategoryId = request.CategoryId,
                    Description = request.CouponTitle,
                    Details = request.CouponDescription,
                    ExpirationDate = request.ExpirationDate,
                    IsActive = false,
                    BusinessId = business.BusinessId
                };
                await _productRepository.AddCoupon(coupon);

                // מעדכנים את הבקשה ל-Approved
                request.Status = "Approved";
                await _couponRequestRepository.UpdateAsync(request);

                var resetLink = $"http://localhost:5173/create-password?token={encodedToken}";

                await _emailService.SendEmailAsync(
                    to: consumer.Email,
                    subject: "יצירת סיסמה לחשבון",
                    body: $"שלום {consumer.Name},<br><br>" +
                          $"הרשמתך התקבלה בהצלחה! מה שנשאר זה ליצור סיסמה לחשבון. <br>" +
                          $"לחצו על הקישור כדי ליצור סיסמה:"+
                          $"<a href='{resetLink}'>יצירת סיסמה</a><br><br>" +
                          "הקישור תקף למשך שעה."
                );


                return _mapper.Map<ProductDto>(coupon);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while approving the coupon request", ex);
            }
        }
    }
}
