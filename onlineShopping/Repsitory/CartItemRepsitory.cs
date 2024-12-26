using Data.Data;
using Data.DTO;
using Data.DTO.CartItem;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class CartItemRepsitory : ICartItem
    {
        private readonly AppDbContext context;

        public CartItemRepsitory(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddCartItemAsync(CartItem cartitem,int cartid)
        {
            var existingItem = await context.cartItems
        .FirstOrDefaultAsync(ci => ci.CartId == cartid && ci.Product.ProductId == cartitem.ProId);

            if (existingItem != null)
            {
                // Item exists, increase the quantity
                existingItem.Quantity += cartitem.Quantity;
                existingItem.TotalPrice += cartitem.Quantity * existingItem.Product.Price; // Update the total price
            }
            else
            {
                // Item does not exist, add it as a new item
                cartitem.CartId = cartid;
                await context.cartItems.AddAsync(cartitem);
            }

            await context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItembyid(string userid, int productid)
        {
            var product = await context.cartItems.Include(x => x.Product).Where(x => x.Cart.userid == userid).FirstOrDefaultAsync(x => x.Product.ProductId == productid);
            return product;
        }

        public async Task RemoveFromCartitem(string userid, int ProductId)
        {
            var product = await context.cartItems.Include(x => x.Product).
                Where(x => x.Cart.userid == userid).FirstOrDefaultAsync(x => x.Product.ProductId == ProductId);
            if (product != null)
            {
                context.cartItems.Remove(product); // Remove the item
                await context.SaveChangesAsync();   // Persist changes
            }
        }

        public async Task updatecartitem(string userid, CartItem cartitem)
        {
            var cart = await context.carts.Include(_cartitem => _cartitem.CartItems).ThenInclude(_product => _product.Product).ThenInclude(x=>x.Coupon)
                 .FirstOrDefaultAsync(key => key.userid == userid);
            cart?.CartItems?.FirstOrDefault(x => x.ProId == cartitem.ProId); // Missing assignment of the found item.
            await context.SaveChangesAsync();

        }
        public async Task ClearCartAsync(string userId)
        {
            var cartItems = context.cartItems.Where(c => c.Cart.userid == userId);
            context.cartItems.RemoveRange(cartItems);
            await context.SaveChangesAsync();
        }


    }
}
