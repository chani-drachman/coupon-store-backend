using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProducts();
        public Task<Product> GetProductById(int id);
        public Task<Product> AddCoupon(Product coupon);
        public Task<bool> DeleteProduct(int id);
        public Task<List<Product>> GetProductsByCategory(int categoryId);
        public Task<Product?> UpdateCoupon(int productId, Product updatedCoupon);


    }
}
