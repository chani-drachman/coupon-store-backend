using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetAllProducts();
        public Task<List<ProductDto>> GetActiveCoupons();
        public Task<ProductDto> GetProductById(int id);
        public Task<ProductDto> AddProduct(BusinessDto business, ProductDto product);
        public Task<bool> DeleteProduct(int id);
        public Task<List<ProductDto>> GetProductsByCategory(int categoryId);
        public Task<ProductDto?> UpdateCoupon(int productId, ProductDto couponDto);
        public Task<ProductDto> ToggleCouponActive(int productId);


    }
}
