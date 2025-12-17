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
    public class CouponRequestRepository: ICouponRequestRepository
    {
        private readonly CouponShopContext _context;

        public CouponRequestRepository(CouponShopContext context)
        {
            _context = context;
        }

        public async Task<List<CouponRequest>> GetAllCouponRequests()
        {
            try
            {
                return await _context.CouponRequests.ToListAsync();


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving requests", ex);
            }
        }

        public async Task<CouponRequest> AddCouponRequest(CouponRequest request)
        {
            try
            {
                await _context.CouponRequests.AddAsync(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding a coupon request.", ex);
            }
        }
        public async Task<CouponRequest?> GetCouponRequestById(int id){

            return await _context.CouponRequests.FirstOrDefaultAsync(cr => cr.RequestId == id);
        }
        public async Task UpdateAsync(CouponRequest request)
        {
            _context.CouponRequests.Update(request);
            await _context.SaveChangesAsync();
        }



    }
}
