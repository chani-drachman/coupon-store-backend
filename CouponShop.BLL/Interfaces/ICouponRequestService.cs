using CouponShop.DAL.Entities;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface ICouponRequestService
    {
        public Task<List<CouponRequestDto>> GetAllCouponRequests();
        public Task<CouponRequestDto> AddCouponRequest(CouponRequestDto dto);
        public Task<ProductDto> ApproveCouponRequest(int requestId);
    }
}
