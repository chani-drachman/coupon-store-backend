using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllCategories();

        public Task<CategoryDto> GetCategoryById(int id);
        public Task<CategoryDto> AddCategory(CategoryDto categoryDetails);

    }
}
