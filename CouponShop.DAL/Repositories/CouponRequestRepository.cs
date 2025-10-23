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
                _context.CouponRequests.Add(request);
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

        public async Task<Product> AddBusinessAndCoupon(CouponRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // בודקים אם העסק כבר קיים
                var existingBusiness = await _context.Businesses
                    .FirstOrDefaultAsync(b =>
                        b.Name == request.BusinessName &&
                        b.Email == request.BusinessEmail);

                Business business;
                if (existingBusiness != null)
                {
                    business = existingBusiness;
                }
                else
                {
                    // מוסיפים עסק חדש
                    business = new Business
                    {
                        Name = request.BusinessName,
                        Email = request.BusinessEmail,
                        Phone = request.BusinessPhone
                    };
                    await _context.Businesses.AddAsync(business);
                    await _context.SaveChangesAsync();
                }

                // מוסיפים את הקופון (Product) ומקשרים לעסק
                var product = new Product
                {
                    Description = request.CouponTitle,
                    Details = request.CouponDescription,
                    Price = 20,
                    IsActive = true,
                    BusinessId = business.BusinessId,
                    CategoryId = request.CategoryId ?? 17

                };
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return product;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
