using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class CartRepsitory : ICart
    {
        private readonly AppDbContext context;

        public CartRepsitory(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Cart> CartExistsAsync(string userId)
        {
            var data = await context.carts.FirstOrDefaultAsync(x => x.userid == userId);
            return data;
        }

        public async Task CreateCartAsync(Cart cart)
        {
            await context.carts.AddAsync(cart);
            await context.SaveChangesAsync();
        }

        public  async Task DeleteCartItem(string userid, int productid)
        {
            var cart =await GetCartByUserIdAsync(userid);
            var cartitem = cart.CartItems.FirstOrDefault(x => x.ProId == productid);
            if (cartitem != null)
            {
                cart.CartItems.Remove(cartitem);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            var data = await context.carts.Include(_cartitem => _cartitem.CartItems).ThenInclude(_product => _product.Product).ThenInclude(x=>x.Coupon).
                 FirstOrDefaultAsync(key => key.userid == userId);
            return data;
        }

       

    }
}
