using AutoMapper;
using CouponShop.API.Models;
using CouponShop.DTO;

namespace CouponShop.API.Mapping
{
    public class ApiMappingProfile: Profile
    {
        public ApiMappingProfile() { 
         CreateMap<BusinessRequest, BusinessDto>();
            CreateMap<ProductRequest, ProductDto>();
            CreateMap<CouponRequest, ProductDto>();
            CreateMap<ConsumerRequest, ConsumerDto>().ReverseMap();
            CreateMap<UpdateConsumerRequest, ConsumerDto>();
            CreateMap<CategoryRequest, CategoryDto>();

            CreateMap<OrderItemRequest, OrderItemDto>()
      .ForMember(dest => dest.CouponCodes, opt => opt.Ignore()); // הקופונים נוצרו אוטומטית ב-Service
            CreateMap<OrderRequest, OrderDto>();
            CreateMap<AddCouponRequest, AddCouponRequestDto>();


        }
    }
}
