using CouponShop.DAL.Context;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DAL.Repositories
{
    public class ConsumerRepository: IConsumerRepository
    {
        private readonly CouponShopContext _context;

        public ConsumerRepository(CouponShopContext context)
        {
            _context = context;
        }
        public async Task<Consumer?> GetConsumerByEmail(string email)
        {
            return await _context.Consumers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Consumer> AddConsumer(Consumer consumer){
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer), "consumer cannot be null");

            consumer.CreatedAt = DateTime.UtcNow;

            try
            {
               await _context.Consumers.AddAsync(consumer);
                await _context.SaveChangesAsync();
                return consumer;
            }
            catch (DbUpdateException ex)
            {
                // טיפול בשגיאות מסד נתונים ספציפיות
                throw new Exception("An error occurred while adding the consumer to the database.", ex);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כלליות
                throw new Exception("An unexpected error occurred while adding the consumer.", ex);
            }

        }
        public async Task<Consumer?> ConsumerLogin(string email, string password)
        {
            var consumer = await _context.Consumers.FirstOrDefaultAsync(c => c.Email == email);
            if (consumer == null)
                return null;

            var hashedPassword = HashPassword(password);
            if (consumer.PasswordHash != hashedPassword)
                return null;

            return consumer;
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<Consumer?> UpdateConsumer(int consumerId, Consumer updatedConsumer)
        {
            if (updatedConsumer == null)
                throw new ArgumentNullException(nameof(updatedConsumer), "Updated consumer cannot be null.");

            try
            {
                var existingConsumer = await _context.Consumers.FindAsync(consumerId);
                if (existingConsumer == null)
                    return null;

                existingConsumer.Name = updatedConsumer.Name;
                existingConsumer.Email = updatedConsumer.Email;
                existingConsumer.Phone = updatedConsumer.Phone;
                existingConsumer.Address = updatedConsumer.Address;

         

                await _context.SaveChangesAsync();
                return existingConsumer;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the consumer in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the consumer.", ex);
            }
        }

        public async Task<Consumer?> GetConsumerById(int consumerId)
        {
            return await _context.Consumers.FindAsync(consumerId);
        }

        public async Task<Consumer> UpdatePassword(Consumer consumer, string newPasswordHash)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));

            consumer.PasswordHash = newPasswordHash;
            await _context.SaveChangesAsync();
            return consumer;
        }

        public async Task<Consumer?> GetByResetToken(string hashedToken)
        {
            return await _context.Consumers
                .FirstOrDefaultAsync(c => c.ResetPasswordToken == hashedToken);
        }

        public async Task UpdateAsync(Consumer consumer)
        {
            _context.Consumers.Update(consumer);
            await _context.SaveChangesAsync();
        }


    }
}
