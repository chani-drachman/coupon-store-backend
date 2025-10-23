using CouponShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Interfaces
{
    public interface IConsumerRepository
    {
        public Task<Consumer?> GetConsumerByEmail(string email);
        public Task<Consumer> AddConsumer(Consumer consumer);
        public Task<Consumer?> ConsumerLogin(string email, string password);
        public Task<Consumer?> UpdateConsumer(int consumerId, Consumer updatedConsumer);
        public Task<Consumer?> GetConsumerById(int consumerId);

        public Task<Consumer> UpdatePassword(Consumer consumer, string newPasswordHash);


    }
}
