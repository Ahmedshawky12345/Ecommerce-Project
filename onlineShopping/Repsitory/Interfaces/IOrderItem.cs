using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface IOrderItem
    {
        Task AddAsync(OrderItem entity);
        Task UpdateAsync(string userid , OrderItem orderitem);
        Task<OrderItem> GetByOrderIdAndProductIdAsync(int orderId, int productId);
        public Task DeleteAsync(int productId, string userId);
    }
}
