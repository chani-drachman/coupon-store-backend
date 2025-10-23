using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategories();

        public Task<Category> GetCategoryById(int id);
        public Task<Category> AddCategory(Category category);
    }
}
