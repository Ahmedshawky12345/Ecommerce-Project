public class ApplyCouponDTO
{
    public int CouponId { get; set; }  // Add CouponId for applying coupon by ID
    public string CouponCode { get; set; }  // Retain CouponCode if you still want to use the code
}

public class CouponDTO
{
    public string Name { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ExpiryDate { get; set; }
}
