using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class Business
{
    public int BusinessId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? ConsumerId { get; set; }

    public virtual Consumer? Consumer { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
