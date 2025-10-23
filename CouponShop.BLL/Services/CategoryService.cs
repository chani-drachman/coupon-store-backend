using AutoMapper;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DAL.Repositories;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryDto> AddCategory(CategoryDto categoryDetails)
        {
            try
            {
                var toAddCategory = _mapper.Map<Category>(categoryDetails);
                var newCategory = await _categoryRepository.AddCategory(toAddCategory);
                return _mapper.Map<CategoryDto>(newCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategories();
                if (categories == null || !categories.Any()) 
                    return new List<CategoryDto>();
                
                return _mapper.Map<List<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving categories from database.", ex);
            }
            }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex) { throw new Exception("An error occured while retrieving category", ex); }


        }
    }
}
