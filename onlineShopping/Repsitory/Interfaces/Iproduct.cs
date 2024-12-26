using Data.DTO;
using Data.Model;

namespace onlineShopping.Repsitory.Interfaces
{
    public interface Iproduct
    {
        Task<List<Product>> GetproductbycategoryId(int id);
        Task<IEnumerable<Product>> Getnewerproduct();
        Task<IEnumerable<Product>> GetproductHasCoupon();
      Task<PagedResponse<Product>>  GetAllproductwithPagination(int pagenumber, int pageSize);
        Task<List<Product>> SearchProducts(string quary);
    }
}
