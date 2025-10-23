using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface ICouponRequestRepository
    {
        public Task<List<CouponRequest>> GetAllCouponRequests();
        public Task<CouponRequest> AddCouponRequest(CouponRequest request);
        public Task<CouponRequest?> GetCouponRequestById(int id);
        public Task UpdateAsync(CouponRequest request);
        public Task<Product> AddBusinessAndCoupon(CouponRequest request);
    }
}
