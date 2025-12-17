using AutoMapper;
using CouponShop.BLL.Helpers;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DAL.Interfaces;
using CouponShop.DAL.Repositories;
using CouponShop.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Services
{

    public class ConsumerService: IConsumerService
    {

        private readonly IConsumerRepository _consumerRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IMapper _mapper;

        public ConsumerService(IConsumerRepository consumerRepository,IBusinessRepository businessRepository,
            IPasswordHelper passwordHelper, IMapper mapper)
        {
            _consumerRepository = consumerRepository;
            _businessRepository = businessRepository;
            _passwordHelper = passwordHelper;
            _mapper = mapper;
        }
        public async Task<LoginResponse> AddConsumer(ConsumerDto consumerDetails){

            var existing = await _consumerRepository.GetConsumerByEmail(consumerDetails.Email!);
            if (existing != null)
                throw new Exception("A consumer with this email already exists.");

           var consumerEntity = _mapper.Map<Consumer>(consumerDetails);
            if(consumerDetails.Password!=null)
            consumerEntity.PasswordHash = _passwordHelper.HashPassword(consumerDetails.Password);

            var addedConsumer = await _consumerRepository.AddConsumer(consumerEntity);
            var consumerDto=_mapper.Map<ConsumerDto>(addedConsumer);
            return new LoginResponse
            {
                token = await GenerateToken(consumerDto.ConsumerId),
                Consumer = consumerDto
            };

        }



        public async Task<LoginResponse> Login(string email, string password)
        {

            var user = await _consumerRepository.GetConsumerByEmail(email);
            if (user == null || !_passwordHelper.VerifyPassword(password, user.PasswordHash)) { return null; }

           

            var consumerDto = _mapper.Map<ConsumerDto>(user);

            return new LoginResponse
            {
                token = await GenerateToken(consumerDto.ConsumerId),
                Consumer = consumerDto,
            };

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
           
            if (_passwordHelper.VerifyPassword(currentPassword, consumer.PasswordHash))
                throw new Exception("Current password is incorrect.");

            var newHash = _passwordHelper.HashPassword(newPassword);
            await _consumerRepository.UpdatePassword(consumer, newHash);

            return true;
        }
        public async Task<string> GenerateToken(int consumerId)
        {
            var consumer = await _consumerRepository.GetConsumerById(consumerId);
            if (consumer == null)
                throw new Exception("Consumer not found");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("CouponShopSecretKey2025ByChaniDrachman854");

            
            var claims = new List<Claim>
             {
               new Claim(ClaimTypes.NameIdentifier, consumerId.ToString()),
               new Claim(ClaimTypes.Role, consumer.Role)
             };

            var business = await _businessRepository.GetBusinessByConsumerId(consumerId);

            if (business != null)
            {
                claims.Add(new Claim("BusinessId", business.BusinessId.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "CouponShopAPI",
                Audience = "CouponShopUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponse> CreatePassword(CreatePasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
                throw new ArgumentException("Token is required");

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                throw new ArgumentException("Password is required");

            // 1. Hash the incoming token
            var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Token)));
            
            // 2. Find consumer
            var consumer = await _consumerRepository.GetByResetToken(hashedToken);
            if (consumer == null)
                throw new KeyNotFoundException("Invalid token");

            // 3. Check expiry
            if (consumer.ResetPasswordExpiry == null ||
                consumer.ResetPasswordExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Token has expired");

           
            if (dto.NewPassword.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");

    
            consumer.PasswordHash = _passwordHelper.HashPassword(dto.NewPassword);

            // Clear reset token
            consumer.ResetPasswordToken = null;
            consumer.ResetPasswordExpiry = null;

            // Update DB
            await _consumerRepository.UpdateAsync(consumer);


            return new LoginResponse {
                Consumer = _mapper.Map<ConsumerDto>(consumer),
                token = await GenerateToken(consumer.ConsumerId) };
           
        }





    }
}
