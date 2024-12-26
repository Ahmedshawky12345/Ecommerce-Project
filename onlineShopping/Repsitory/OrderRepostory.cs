using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace onlineShopping.Repsitory
{
    public class OrderRepostory :  Iorder
    {
        private readonly AppDbContext context;

        public OrderRepostory(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Order order)
        {
            await context.AddAsync(order);
            await context.SaveChangesAsync();
        }

        public async Task DeleteOrderItem(string userId, int ProductId)
        {
            var order = await GetOrdersByUserIdAsync(userId);

            var orderitem = order.orderItems.FirstOrDefault(x => x.ProductId == ProductId);
            if(orderitem!= null)
            {
                order.orderItems.Remove(orderitem);
                context.SaveChanges();
            }

        }

      

       

        public async Task<Order> GetAllOrderDetiles(string userid)
        {
            var data = await context.orders.Include(_orderitem => _orderitem.orderItems)
                .ThenInclude(_product => _product.product).FirstOrDefaultAsync(x=>x.UserId==userid);
            return data;
        }

        public Task<Order> Getorderbyid(int id)
        {
            var data = context.orders.Include(x => x.orderItems).ThenInclude(x => x.product).FirstOrDefaultAsync(x => x.Id == id);
            return data;
        }

        public async Task<Order> GetOrdersByUserIdAsync(string userId)
        {
             var data = await context.orders.Include(x=>x.orderItems).FirstOrDefaultAsync(x => x.UserId == userId);
            await context.SaveChangesAsync();
            return data;
        }

        public async Task UpdateAsync(Order order)
        {
            context.orders.Update(order); 
            await context.SaveChangesAsync();
        }
       
    }
}
