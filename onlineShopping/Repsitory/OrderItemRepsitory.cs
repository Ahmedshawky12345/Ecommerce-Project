using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class OrderItemRepsitory :IOrderItem
    {
        private readonly AppDbContext context;

        public OrderItemRepsitory(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(OrderItem entity)
        {
            await context.orderItems.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string userid, OrderItem orderitem)
        {
            var order = await context.orders.Include(x=>x.orderItems).FirstOrDefaultAsync(x => x.UserId == userid);
            order.orderItems.FirstOrDefault(x => x.ProductId == orderitem.ProductId);

            await context.SaveChangesAsync();
        }
        public async Task<OrderItem> GetByOrderIdAndProductIdAsync(int orderId, int productId)
        {
            return await context.orderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);
        }
        public async Task DeleteAsync(int productId, string userId)
        {
            var order = await context.orders.Include(x => x.orderItems).FirstOrDefaultAsync(x => x.UserId == userId);
            if (order != null)
            {
                var orderItem = order.orderItems.FirstOrDefault(x => x.ProductId == productId);
                if (orderItem != null)
                {
                    order.orderItems.Remove(orderItem);
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
