using CouponShop.DAL.Context;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Repositories
{
    public class CouponCodeRepository : ICouponCodeRepository
    {
        private readonly CouponShopContext _context;

        public CouponCodeRepository(CouponShopContext context)
        {
            _context = context;
        }
        public async Task<CouponCode?> GetCouponCodeByCode(string code)
        {
            try
            {
                return await _context.CouponCodes
                  .Include(c => c.OrderItem)
                      .ThenInclude(oi => oi.Product)
                  .Include(c => c.OrderItem)
                      .ThenInclude(oi => oi.Order)
                          .ThenInclude(o => o.Consumer)
                  .FirstOrDefaultAsync(c => c.Code == code);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving the coupon.", ex);
            }
        }
        public async Task<CouponCode?> GetCouponCodeById(int couponId){
            try
            {
                return await _context.CouponCodes
        .Include(c => c.OrderItem)
            .ThenInclude(oi => oi.Product)
        .FirstOrDefaultAsync(c => c.CouponId == couponId);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving the coupon.", ex);
            }
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
