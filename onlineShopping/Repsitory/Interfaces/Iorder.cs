using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface Iorder
    {
        Task<Order> GetOrdersByUserIdAsync(string userId);
        Task DeleteOrderItem(string userId,int ProductId);
          Task<Order> GetAllOrderDetiles(string userid);
        Task UpdateAsync(Order order);
        Task AddAsync(Order order);
        Task<Order> Getorderbyid(int id);
   


    }
}
