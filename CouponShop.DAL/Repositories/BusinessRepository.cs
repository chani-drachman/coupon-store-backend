using CouponShop.DAL.Context;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly CouponShopContext _context;

        public BusinessRepository(CouponShopContext context)
        {
            _context = context;
        }
        public async Task<Business> AddBusiness(Business business)
        {
            if (business == null)
                throw new ArgumentNullException(nameof(business), "business cannot be null.");

            try
            {
                await _context.Businesses.AddAsync(business);
                await _context.SaveChangesAsync();
                return business;

            }
            catch (DbUpdateException ex)
            {
                // טיפול בשגיאות מסד נתונים ספציפיות
                throw new Exception("An error occurred while adding the business to the database.", ex);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while adding the business.", ex);
            }
        }
        public async Task<Business?> GetBusinessByEmail(string email)
        {
            return await _context.Businesses
                              .FirstOrDefaultAsync(b => b.Email == email);


        }
        public async Task<List<Business>> GetAllBusiness()
        {
            try
            {
                return await _context.Businesses.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving businesses", ex);
            }
        }

        public async Task<Business?> UpdateBusiness(int  businessId, Business updatedBusiness)
        {
            if (updatedBusiness == null)
                throw new ArgumentNullException(nameof(updatedBusiness), "Updated business cannot be null.");
            try
            {
                var existingBusiness = await _context.Businesses.FindAsync(businessId);
                if (existingBusiness == null)
                    return null;

                existingBusiness.Email = updatedBusiness.Email;
                existingBusiness.Phone = updatedBusiness.Phone;
                existingBusiness.Address = updatedBusiness.Address;
                existingBusiness.Name = updatedBusiness.Name;

                await _context.SaveChangesAsync();

                return existingBusiness;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the business in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the business.", ex);
            }
        }
    }
}
