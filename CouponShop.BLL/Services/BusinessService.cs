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
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IMapper _mapper;
 
        public BusinessService(IBusinessRepository businessRepository, IMapper mapper)
        {
            _businessRepository = businessRepository;   
            _mapper = mapper;
        }
        public async Task<BusinessDto> AddBusiness(BusinessDto businessDetails)
        {
            try
            {
                var toAddBusiness = _mapper.Map<Business>(businessDetails);
                var newBusiness= await _businessRepository.AddBusiness(toAddBusiness);
                return _mapper.Map<BusinessDto>(newBusiness);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            }
        public async Task<BusinessDto?> GetBusinessByEmail(string email)
        {
            var business = await _businessRepository.GetBusinessByEmail(email);

            if (business == null)
                return null; // לא נמצא עסק

            return _mapper.Map<BusinessDto>(business);
        }


        public async Task<List<BusinessDto>> GetAllBusinesses()
        {
            try
            {
                var businesses = await _businessRepository.GetAllBusiness();
                return _mapper.Map<List<BusinessDto>>(businesses);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving businesses", ex);
            }
        }

        public async Task<BusinessDto?> UpdateBusiness(int businessId, BusinessDto businessDetails)
        {
            try { 
            var newDetails=_mapper.Map<Business>(businessDetails);
            var business = await _businessRepository.UpdateBusiness(businessId, newDetails);
            if (business == null) return null;

            return _mapper.Map<BusinessDto?>(business);}
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the business with ID {businessId}.", ex);
            }
        }

    }
}
