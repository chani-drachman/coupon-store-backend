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
    public class ProductRepository : IProductRepository 
    {
        private readonly CouponShopContext _context;

        public ProductRepository(CouponShopContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _context.Products.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving products", ex);
            }
        }
        public async Task<List<Product>> GetActiveCoupons(){
            try
            {
                return await _context.Products.Where(c => c.IsActive == true)
                        .Include(c => c.Business)
                        .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving products", ex);
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {

                    throw new KeyNotFoundException($"Product with ID {id} was not found.");
                }

                return product;
            }
            catch (KeyNotFoundException)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the product from the database.", ex);
            }
        }
        public async Task<Product> AddCoupon(Product coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException(nameof(coupon), "Product cannot be null.");

            try
            {
                await _context.Products.AddAsync(coupon); 
                await _context.SaveChangesAsync();  
                return coupon;
            }
            catch (DbUpdateException ex)
            {
                // טיפול בשגיאות מסד נתונים ספציפיות
                throw new Exception("An error occurred while adding the product to the database.", ex);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while adding the product.", ex);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false; // לא נמצא מוצר

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true; // הצלחה
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the product from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the product.", ex);
            }
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products
                                             .Where(p => p.CategoryId == categoryId)
                                             .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products for category ID {categoryId}.", ex);
            }

        }
        public async Task<Product?> UpdateCoupon(int productId, Product updatedCoupon)
        {
            if (updatedCoupon == null)
                throw new ArgumentNullException(nameof(updatedCoupon), "Updated coupon cannot be null.");

            try
            {
                var existingCoupon = await _context.Products.FindAsync(productId);

                if (existingCoupon == null)
                    return null; 

                // עדכון שדות
                existingCoupon.CategoryId = updatedCoupon.CategoryId;
                existingCoupon.Description = updatedCoupon.Description;
                existingCoupon.Details = updatedCoupon.Details;
                existingCoupon.ImageUrl = updatedCoupon.ImageUrl;
                existingCoupon.ExpirationDate = updatedCoupon.ExpirationDate;
                existingCoupon.IsActive = updatedCoupon.IsActive;

                await _context.SaveChangesAsync();

                return existingCoupon;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the coupon in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the coupon.", ex);
            }
        }
        public async Task<Product> ToggleCouponActive(int id)
        {
            var coupon = await _context.Products.FindAsync(id);
            if (coupon == null)
                throw new ArgumentNullException("coupon was not found");
            coupon.IsActive = !coupon.IsActive;
            await _context.SaveChangesAsync();
            return coupon;
        }
            






    }

}
