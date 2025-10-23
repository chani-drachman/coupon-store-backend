using AutoMapper;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{

    public class ConsumerService: IConsumerService
    {

        private readonly IConsumerRepository _consumerRepository;
        private readonly IMapper _mapper;

        public ConsumerService(IConsumerRepository consumerRepository, IMapper mapper)
        {
            _consumerRepository = consumerRepository;
            _mapper = mapper;
        }
        public async Task<ConsumerDto> AddConsumer(ConsumerDto consumerDetails){

            var existing = await _consumerRepository.GetConsumerByEmail(consumerDetails.Email!);
            if (existing != null)
                throw new Exception("A consumer with this email already exists.");

           var consumerEntity = _mapper.Map<Consumer>(consumerDetails);
            consumerEntity.PasswordHash = HashPassword(consumerDetails.Password);

            var addedConsumer = await _consumerRepository.AddConsumer(consumerEntity);

            return _mapper.Map<ConsumerDto>(addedConsumer);

        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public async Task<ConsumerDto?> ConsumerLogin(string email, string password)
        {
            var consumer = await _consumerRepository.ConsumerLogin(email, password);
            if (consumer == null)
                return null;

            return _mapper.Map<ConsumerDto>(consumer);
        }

        public async Task<ConsumerDto?> UpdateConsumer(int consumerId, ConsumerDto consumerDetails)
        {
            if (consumerDetails == null)
                throw new ArgumentNullException(nameof(consumerDetails));

            try
            {
                // המרה ל-Entity
                var consumerEntity = _mapper.Map<Consumer>(consumerDetails);

                // קריאה ל-Repository
                var updatedConsumer = await _consumerRepository.UpdateConsumer(consumerId, consumerEntity);

                if (updatedConsumer == null) return null;

                // המרה חזרה ל-DTO
                return _mapper.Map<ConsumerDto>(updatedConsumer);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating consumer with ID {consumerId}.", ex);
            }
        }
        public async Task<bool> ChangePassword(int consumerId, string currentPassword, string newPassword)
        {
            var consumer = await _consumerRepository.GetConsumerById(consumerId);
            if (consumer == null) return false;

            // בדיקת סיסמה נוכחית
            var currentHash = HashPassword(currentPassword);
            if (consumer.PasswordHash != currentHash)
                throw new Exception("Current password is incorrect.");

            var newHash = HashPassword(newPassword);
            await _consumerRepository.UpdatePassword(consumer, newHash);

            return true;
        }

        public async Task<string> GenerateJwtToken(int consumerId)
        {
            var consumer = await _consumerRepository.GetConsumerById(consumerId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("CouponShopSecretKey2025ByChaniDrachman854"); // מפתח סודי, שמור ב-appsettings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("ConsumerId", consumerId.ToString()), new Claim(ClaimTypes.Role, consumer.Role) }),
                Expires = DateTime.UtcNow.AddHours(2), // זמן תפוגה של 2 שעות
                Issuer = "CouponShopAPI", // צריך להתאים למה שהגדרת ב-Program.cs
                Audience = "CouponShopUsers",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




    }
}
