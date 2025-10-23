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
using static Azure.Core.HttpHeader;

namespace CouponShop.BLL.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository,IBusinessRepository businessRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _businessRepository=businessRepository;
            _mapper = mapper;
        }

     

        public async Task<List<ProductDto>> GetAllProducts()
        {
            try
            {
                var products= await _productRepository.GetAllProducts();
                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex) {
                throw new Exception("An error occured while retrieving products", ex);
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            try {
                var product = await _productRepository.GetProductById(id);
                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex) { throw new Exception("An error occured while retrieving product", ex); }
        }

        public async Task<ProductDto> AddProduct(BusinessDto business, ProductDto product)
        {
            var existingBusiness = await _businessRepository.GetBusinessByEmail(business.Email);
            Business businessEntity;
            if (existingBusiness != null)
            {
                businessEntity = existingBusiness;
            }
            else
            {
                var newBusiness = _mapper.Map<Business>(business);
                businessEntity = await _businessRepository.AddBusiness(newBusiness);
            }
            try
            {


                var productEntity = _mapper.Map<Product>(product);
                productEntity.BusinessId = businessEntity.BusinessId;
                productEntity.Price = 20;
                var addedProduct = await _productRepository.AddCoupon(productEntity);
                return _mapper.Map<ProductDto>(addedProduct);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var result = await _productRepository.DeleteProduct(id);
                return result; // true אם נמחק, false אם לא נמצא
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to delete the product.", ex);
            }
        }


        public async Task<List<ProductDto>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);

                if (products == null || !products.Any())
                    return new List<ProductDto>(); // מחזירים רשימה ריקה אם אין מוצרים

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products for category ID {categoryId}.", ex);
            }
        }

        public async Task<ProductDto?> UpdateCoupon(int productId, ProductDto couponDto)
        {
            try
            {
                var couponEntity = _mapper.Map<Product>(couponDto);
                var updatedCoupon = await _productRepository.UpdateCoupon(productId, couponEntity);

                if (updatedCoupon == null)
                    return null;

                return _mapper.Map<ProductDto>(updatedCoupon);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the coupon with ID {productId}.", ex);
            }
        }




    }
}
