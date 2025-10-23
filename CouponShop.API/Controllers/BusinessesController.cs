using AutoMapper;
using CouponShop.API.Models;
using CouponShop.BLL.Interfaces;
using CouponShop.DAL.Entities;
using CouponShop.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IMapper _mapper;

        public BusinessesController(IBusinessService businessService, IMapper mapper)
        {
          _businessService = businessService;
            _mapper = mapper;
        }
        //api/business
        [HttpPost]

        public async Task<ActionResult<BusinessDto>> AddBusiness([FromBody] BusinessRequest businessDetails)
        {
            try
            {
                var newBusiness = await _businessService.AddBusiness(_mapper.Map<BusinessDto>(businessDetails));
                return Ok(newBusiness);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating new business", ex);
            }

            }

        //api/business/{id}
        [HttpGet("{email}")]
        public async Task<ActionResult<BusinessDto>> GetBusinessByEmail(string email)
        {
            var business = await _businessService.GetBusinessByEmail(email);

            if (business == null)
                return NotFound(new { message = $"Business with email {email} not found" });

            return Ok(business);
        }


        //api/business
        [HttpGet]
        public async Task<ActionResult<List<BusinessDto>>> GetAllBusinesses()
        {
            try
            {
                var businesses = await _businessService.GetAllBusinesses();
                return Ok(businesses);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving business", ex);
            }

            }

        //api/businesses/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BusinessDto>> UpdateBusiness(int id, [FromBody] BusinessRequest businessRequest)
        {
            try
            {
                var business = _mapper.Map<BusinessDto>(businessRequest);
                var updatedBusiness = await _businessService.UpdateBusiness(id, business);
                if (updatedBusiness == null) return NotFound(new { Message = $"Business with ID {id} not found." });
                return Ok(updatedBusiness);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
