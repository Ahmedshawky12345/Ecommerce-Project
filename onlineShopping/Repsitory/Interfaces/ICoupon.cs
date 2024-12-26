using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface ICoupon
    {
        Task ApplayCouponToProduct(Coupon entity);
    }
}
