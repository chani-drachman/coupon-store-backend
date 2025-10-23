using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface IBusinessService
    {
        public Task<BusinessDto> AddBusiness(BusinessDto businessDetails);
        public Task<BusinessDto?> GetBusinessByEmail(string email);
        public Task<List<BusinessDto>> GetAllBusinesses();
        public Task<BusinessDto?> UpdateBusiness(int businessId, BusinessDto businessDetails);

    }
}
