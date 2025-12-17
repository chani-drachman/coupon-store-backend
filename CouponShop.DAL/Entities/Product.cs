using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public int BusinessId { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Business Business { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
