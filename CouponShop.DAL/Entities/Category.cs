using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CouponRequest> CouponRequests { get; set; } = new List<CouponRequest>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
