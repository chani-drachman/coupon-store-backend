using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface ICouponCodeRepository
    {
        public Task<CouponCode?> GetCouponCodeByCode(string code);
        public Task<CouponCode?> GetCouponCodeById(int couponId);
        public Task SaveChangesAsync();
    }
}
