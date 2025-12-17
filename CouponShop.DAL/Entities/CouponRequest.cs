using System;
using System.Collections.Generic;

namespace CouponShop.DAL.Entities;

public partial class CouponRequest
{
    public int RequestId { get; set; }

    public string BusinessName { get; set; } = null!;

    public string BusinessEmail { get; set; } = null!;

    public string BusinessPhone { get; set; } = null!;

    public string CouponTitle { get; set; } = null!;

    public string? CouponDescription { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal? DiscountValue { get; set; }

    public string? ConditionText { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CategoryId { get; set; }

    public string? PrivateName { get; set; }

    public string? BusinessAddress { get; set; }

    public virtual Category Category { get; set; } = null!;
}
