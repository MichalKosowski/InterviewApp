using System.Collections.Generic;
using System.Threading.Tasks;
using InterviewApp.Core;
using InterviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewApp.DataAccess
{
    public class EntityFrameworkProductsRepository : IProductsRepository
    {
        private readonly ProductsContext _context;

        public EntityFrameworkProductsRepository(ProductsContext context)
        {
            _context = context;
        }

        public Task AddNewAsync(Product product)
        {
            _context.Products.Add(product);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return _context.SaveChangesAsync();
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<Product> GetByIdAsync(long id)
        {
            return _context.Products.FindAsync(id);
        }

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return _context.SaveChangesAsync();
        }
    }
}
