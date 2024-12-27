using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class CouponRegsitory : IRepstory<Coupon>,ICoupon
    {
        private readonly AppDbContext context;

        public CouponRegsitory(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Coupon entity)
        {
            context.coupons.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task ApplayCouponToProduct(Coupon entity)
        {
            //var Products= await context.products.Where(p=>entity.products.Select(x=>x.ProductId).Contains(p.ProductId)).ToListAsync();
            //if(p)
            await context.coupons.AddAsync(entity);
            await context.SaveChangesAsync();   
        }

        public  void Delete(Coupon entity)
        {
            context.coupons.Remove(entity);
             context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            var data = await context.coupons.ToListAsync();
            return data;
        }

        public async Task<Coupon> GetByIdAsync(int id)
        {
            var data = await context.coupons.Include(x=>x.products).FirstOrDefaultAsync(x => x.CouponId == id);
            return data;
        }

        public  void Update(Coupon entity)
        {
            context.coupons.Update(entity);
             context.SaveChangesAsync();
        }
        public async Task RemoveCouponFromProduct(int productId)
        {
            var product = await context.products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null && product.CoupnId != null)
            {
                product.CoupnId = null; // Remove the coupon from the product
                context.products.Update(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Coupon> GetCouponByCodeAsync(string couponCode)
        {
            // Assuming Coupon entity is mapped to a Coupons table in your database
            return await context.coupons
                                 .FirstOrDefaultAsync(c => c.Code == couponCode);
        }
        public async Task<Product> GetCouponByProductIdAsync(int productId)
        {
            var product = await context.products
                .Include(p => p.Coupon) // Eager load the associated coupon
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            return product; // Return the associated coupon or null
        }
    }
}
