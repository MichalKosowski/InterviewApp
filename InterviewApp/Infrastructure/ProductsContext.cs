using InterviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewApp.DataAccess
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
