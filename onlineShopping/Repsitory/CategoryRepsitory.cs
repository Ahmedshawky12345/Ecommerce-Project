using Data.Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Repsitory
{
    public class CategoryRepsitory : IRepstory<Category>
    {
        private readonly AppDbContext context;

        public CategoryRepsitory(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Category entity)
        {
            await context.categories.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Category entity)
        {
            context.categories.Remove(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var data = await context.categories.ToListAsync();
            return data;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category= await context.categories.FirstOrDefaultAsync(_category=>_category.Id==id);
            return category;
        }

        public  void Update(Category entity)
        {
            context.categories.Update(entity);
            context.SaveChanges();
        }
    }
}
