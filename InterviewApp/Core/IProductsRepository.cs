using InterviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewApp.Core
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(long id);
        Task AddNewAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
