using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        //api/categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                if (categories == null || !categories.Any())                                                                                                                                                                                                                                                 
                    return NotFound(new { Message = $"No result was found." });

                return Ok(categories);
            }
            catch (Exception ex) {
                return BadRequest(new { Message = ex.Message });
            }
        }
        //api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCaregoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null) return NotFound();
                        return Ok(category);
            }
            catch (Exception ex) { throw new Exception("An error occured while retrieving category", ex); }
        }

        //api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory(CategoryRequest categoryDetails)
        {
            try
            {
                var categoryDto = _mapper.Map<CategoryDto>(categoryDetails);
                var newCategory = await _categoryService.AddCategory(categoryDto);
                if (newCategory != null) return Ok(newCategory);
                return BadRequest(new { Message = "Failed to create category" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        
    }
}
