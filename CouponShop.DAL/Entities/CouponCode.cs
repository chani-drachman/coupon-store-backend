using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class CouponCode
{
    public int CouponId { get; set; }

    public int OrderItemId { get; set; }

    public string Code { get; set; } = null!;

    public bool? IsRedeemed { get; set; }

    public DateTime? RedeemedAt { get; set; }

    public virtual OrderItem OrderItem { get; set; } = null!;
}
