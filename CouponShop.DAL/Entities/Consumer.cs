using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class Consumer
{
    public int ConsumerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string Role { get; set; } = "User";

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
