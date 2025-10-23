using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public int BusinessId { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? Details { get; set; }

        public string? ImageUrl { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public bool? IsActive { get; set; }


    }
}
