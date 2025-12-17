
using CouponShop.DAL.Interfaces;
using CouponShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Interfaces
{
    public interface IConsumerService
    {

        public Task<LoginResponse> AddConsumer(ConsumerDto consumer);
        public  Task<LoginResponse> Login(string email, string password);
      
        public Task<ConsumerDto?> UpdateConsumer(int consumerId, ConsumerDto consumerDetails);
        public Task<bool> ChangePassword(int consumerId, string currentPassword, string newPassword);
        public Task<string> GenerateToken(int consumerId);
        public Task<LoginResponse> CreatePassword(CreatePasswordDto dto);


    }
}
