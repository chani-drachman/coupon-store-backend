using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;

        }
        //api/product
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            try
            {
                return await _productService.GetAllProducts();
            }
            catch (Exception ex)
            { throw new Exception("An error occured while retrieving products", ex); }
        }
        //api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                 var product= await _productService.GetProductById(id);
                return Ok(product);

            }
            catch (Exception ex) { throw new Exception("An error occured while retrieving product", ex); }
        }

        //api/product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct([FromBody]  ProductRequest productDetails)
        {
            try
            {
                var product =_mapper.Map<ProductDto>(productDetails.Product) ;
                var business=_mapper.Map<BusinessDto>(productDetails.Business) ;
                var result = await _productService.AddProduct(business, product);

                return Ok(result);
            }
            catch (Exception ex) { throw new Exception("An error occured while trying to add product", ex); }


        }
        //api/products
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleted = await _productService.DeleteProduct(id);

                if (!deleted)
                    return NotFound(new { Message = $"Product with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //api/products/category/{id}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategory(categoryId);

                if (products == null || !products.Any())
                    return Ok(new List<ProductDto>());

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        //api/products/{productId}
        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductDto>> UpdateCoupon(int productId, [FromBody] CouponRequest couponRequest)
        {
            try
            {
                
                var couponDto = _mapper.Map<ProductDto>(couponRequest);

                var updatedCoupon = await _productService.UpdateCoupon(productId, couponDto);

                if (updatedCoupon == null)
                    return NotFound(new { Message = $"Coupon with ID {productId} not found." });

                return Ok(updatedCoupon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }



    }
}
