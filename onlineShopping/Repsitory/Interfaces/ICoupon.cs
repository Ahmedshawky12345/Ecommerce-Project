using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface ICoupon
    {
        Task ApplayCouponToProduct(Coupon entity);
        Task RemoveCouponFromProduct(int productId);
        Task<Coupon> GetCouponByCodeAsync(string couponCode);
        Task<Product> GetCouponByProductIdAsync(int productId);
    }
}
