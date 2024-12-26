using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Data.Model;

namespace Data.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
       


        public DbSet<Product> products {  get; set; } 
        public DbSet<Cart> carts { get; set; }  
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; } 
        public DbSet<Category> categories { get;set; }
        public DbSet<Coupon> coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //----------------------- many to many relation ship (products and cart by (cartitem))
            builder.Entity<CartItem>().HasKey(keys => new { keys.CartId, keys.ProId });
            builder.Entity<CartItem>().HasOne(_cart => _cart.Cart).WithMany(_cartitem => _cartitem.CartItems).
                HasForeignKey(key => key.CartId);
            builder.Entity<CartItem>().HasOne(_product => _product.Product).WithMany(_cartitem => _cartitem.CartItems).
                HasForeignKey(key => key.ProId);

            //-------------------------------- one to one relationship(user and Cart)
            builder.Entity<AppUser>().HasOne(_cart => _cart.cart).WithOne(_user => _user.AppUser).
                HasForeignKey<Cart>(key => key.userid);
            //----------------------- many to many relation ship (products and Order by (OrderItem))
            builder.Entity<OrderItem>().HasKey(keys => new { keys.OrderId, keys.ProductId });
            builder.Entity<OrderItem>().HasOne(_order => _order.Order).WithMany(_orderitem => _orderitem.orderItems).
                HasForeignKey(key => key.OrderId);
            builder.Entity<OrderItem>().HasOne(_product => _product.product).WithMany(_orderitem => _orderitem.orderItems).
                HasForeignKey(key => key.ProductId);
            //------------------------- one to many relationship(order and User)-----------------------------------
            builder.Entity<AppUser>().HasMany(_order => _order.orders).WithOne(_user => _user.user).
                HasForeignKey(key => key.UserId);
            //--------------------------- one to many relationship (Category and Product)---------------------------
            builder.Entity<Category>().HasMany(_product => _product.products).WithOne(_category => _category.category).
               HasForeignKey(key => key.CategoryId);
            // ----------------------- one to many relaionship (product and coupon)
            builder.Entity<Coupon>().HasMany(_product => _product.products).WithOne(_coupon => _coupon.Coupon).
               HasForeignKey(key => key.CoupnId);

            base.OnModelCreating(builder);
        }
    }
}
