using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DTO
{
    public class ErrorResponseDto
    {
        public string Error { get; set; }
        public string? Details { get; set; }

        public ErrorResponseDto(string error, string? details = null)
        {
            Error = error;
            Details = details;
        }
    }
}
