using Data.DTO;
using Data.DTO.CartItem;
using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface ICartItem
    {
        Task AddCartItemAsync(CartItem cartItem, int cartid);
       

        Task updatecartitem( string userid,CartItem cartitem);
        Task RemoveFromCartitem(string userid,int ProductId);
        Task<CartItem> GetCartItembyid(string userid,int productid);
        Task ClearCartAsync(string userId);
    }
}
