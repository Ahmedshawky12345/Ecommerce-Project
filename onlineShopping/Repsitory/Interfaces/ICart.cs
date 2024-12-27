using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface ICart
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task CreateCartAsync(Cart cart);
        Task DeleteCartItem(string userid, int productid);
        Task<Cart> CartExistsAsync(string userId);
        Task ClearCartAsync(string userId);


    }
}
