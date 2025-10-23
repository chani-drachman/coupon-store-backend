using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int ConsumerId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public virtual Consumer Consumer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
