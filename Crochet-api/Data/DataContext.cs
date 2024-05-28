using Crochet_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Crochet_api.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}
