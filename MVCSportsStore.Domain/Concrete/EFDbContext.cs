using System.Data.Entity;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } 
    }
}
