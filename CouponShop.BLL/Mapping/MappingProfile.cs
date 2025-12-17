using AutoMapper;
using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Business, BusinessDto>().ReverseMap();
            CreateMap<ConsumerDto, Consumer>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
         .ForMember(dest => dest.ProductDescription,
                    opt => opt.MapFrom(src => src.Product.Description))
         .ForMember(dest => dest.UnitPrice,
                    opt => opt.MapFrom(src => src.UnitPrice))
         .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity))
         .ForMember(dest => dest.CouponCodes,
                    opt => opt.MapFrom(src => src.CouponCodes))
         .ForMember(dest => dest.BusinessName,  
                    opt => opt.MapFrom(src => src.Product.Business.Name));


            CreateMap<CouponCode, CouponCodeDto>();



            // מיפוי של Order -> OrderDto כולל Items
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items,
                           opt => opt.MapFrom(src => src.OrderItems))
                 .ForMember(dest => dest.Consumer,  
               opt => opt.MapFrom(src => src.Consumer));



            CreateMap<CouponRequest, CouponRequestDto>();



        }
    }
}
