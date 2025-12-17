using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface IBusinessRepository
    {
        public Task<Business> AddBusiness(Business business);
        public Task<Business?> GetBusinessByEmail(string email);

        public Task<List<Business>> GetAllBusiness();
        public Task<Business?> UpdateBusiness(int businessId, Business updatedBusiness);
        public Task<Business?> GetBusinessByConsumerId(int consumerId);

    }
}
