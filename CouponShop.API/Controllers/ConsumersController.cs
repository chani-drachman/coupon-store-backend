using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumersController : ControllerBase
    {
        private readonly IConsumerService _consumerService;
        private readonly IMapper _mapper;

        public ConsumersController(IConsumerService consumerService, IMapper mapper)
        {
            _consumerService = consumerService;
            _mapper = mapper;
        }

        //api/consumers
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> AddConsumer([FromBody] ConsumerRequest consumerDetails)
        {
            try
            {
                var consumer = _mapper.Map<ConsumerDto>(consumerDetails);
                
                var response= await _consumerService.AddConsumer(consumer);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }


        }
 
        //api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest request)
        {
            if (request.email == null || request.password == null) return BadRequest("לא הוזנו שם משתמש/סיסמה");
            try
            {
                var response = await _consumerService.Login(request.email, request.password);
                if (response == null)
                    return Unauthorized("שם משתמש או סיסמה שגויים");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while login to your account: {ex.Message}");
            }
        }

        //api/consumer/{id}
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ConsumerDto>> UpdateConsumer( [FromBody] UpdateConsumerRequest updateRequest)
        {
            
                try
                {
                    if (updateRequest == null)
                        return BadRequest("Consumer details are required.");

                // שליפת ה-ID מתוך הטוקן
                var consumerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (consumerIdClaim == null)
                        return Unauthorized(new { Message = "Invalid token." });

                    var consumerId = int.Parse(consumerIdClaim.Value);
   

                // המרה ל-DTO
                var consumerDto = _mapper.Map<ConsumerDto>(updateRequest);

                    var updatedConsumer = await _consumerService.UpdateConsumer(consumerId, consumerDto);

                    if (updatedConsumer == null)
                        return NotFound(new { Message = $"Consumer with ID {consumerId} not found." });

                    return Ok(updatedConsumer);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = ex.Message });
                }
            }
        //api/consumer/{id}/change-password
        [Authorize]
        [HttpPut("/change-password")]
        public async Task<IActionResult> ChangePassword( [FromBody] ChangePasswordRequest request)
        {
            try
            {
                // שליפת ה-ID מתוך הטוקן
                var consumerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (consumerIdClaim == null)
                    return Unauthorized(new { Message = "Invalid token." });

                var consumerId = int.Parse(consumerIdClaim.Value);

                var success = await _consumerService.ChangePassword(consumerId, request.CurrentPassword, request.NewPassword);
                if (!success)
                    return BadRequest(new { Message = "Invalid current password or consumer not found." });

                return Ok(new { Message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("create-password")]
        public async Task<ActionResult<LoginResponse>> CreatePassword([FromBody] CreatePasswordDto dto)
        {
            try
            {
               var response= await _consumerService.CreatePassword(dto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponseDto("Invalid input", ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto("Invalid token", ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, new ErrorResponseDto("Expired token", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto("Server error", ex.Message));
            }
        }
    }

}

