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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CouponShopContext _context;

        public CategoryRepository(CouponShopContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategory(Category category)
        {
            if(category == null)
                throw new ArgumentNullException(nameof(category), "category cannot be null.");

            try
            {
               await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (DbUpdateException ex)
            {
                // טיפול בשגיאות מסד נתונים ספציפיות
                throw new Exception("An error occurred while adding the category to the database.", ex);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while adding the category.", ex);
            }

        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving categories", ex);
            }
            }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} was not found.");
                }
                return category;
            }
            catch (KeyNotFoundException)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the category from the database.", ex);
            }
            
        }
    }
}
