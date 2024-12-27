using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class ProductRepsitory : IRepstory<Product>,Iproduct
    {
        private readonly AppDbContext context;

        public ProductRepsitory(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Product entity)
        {
           await context.products.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public  void Delete(Product entity)
        {
             context.products.Remove(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var Products = await context.products.Include(x=>x.category).ToListAsync();
            return Products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product= await context.products.Include(x => x.category).Include(x=>x.Coupon).FirstOrDefaultAsync(_proudct=>_proudct.ProductId==id);
            return product;
        }

        public async Task<IEnumerable<Product>> Getnewerproduct()
        {
            var newerproducts = await context.products.OrderByDescending(p => p.createdate).Take(10).ToListAsync();
            return newerproducts;
        }

        public async Task<List<Product>> GetproductbycategoryId(int id)
        {
            var product = await context.products.Include(_category => _category.category).Where(x => x.CategoryId == id).ToListAsync();
            return product;
        }

        public  void Update(Product entity)
        {
            context.products.Update(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Product>> GetproductHasCoupon()
        {
            var products = await context.products.Include(x => x.Coupon).Where(x => x.CoupnId>0).ToListAsync();
            return products;
        }

        public async Task<PagedResponse<Product>> GetAllproductwithPagination(int pagenumber, int pageSize)
        {
            // for calculate the how many sikp prodcut to reach the page has products
            var sikp = (pagenumber - 1) * pageSize;
            var Totalitem = await context.products.CountAsync();
            var products= await context.products.Include(x=>x.category).Skip(sikp).Take(pageSize).ToListAsync();

            // will do mapping 
            var PagedResponse = new PagedResponse<Product>
            {
                Items = products,
                PageNumber = pagenumber,
                PageSize = pageSize,
                TotalItems = Totalitem,
                TotalPages = (int)Math.Ceiling(Totalitem / (double)pageSize)
            };

            return PagedResponse;





        }

        public async Task<List<Product>> SearchProducts(string quary)
        {
            var products = await context.products.
                Where(_quary => _quary.Name.Contains(quary) || _quary.Descrption.Contains(quary)).ToListAsync();
            return products;
        }

        public async Task<List<Product>> GetProductsWithCouponsAsync(List<int> productIds)
        {
            return await context.products
                .Where(p => productIds.Contains(p.ProductId))
                .Include(p => p.Coupon) // Include coupon data
                .ToListAsync();
        }
    }
}
