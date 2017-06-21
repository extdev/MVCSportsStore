using System.Collections.Generic;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private readonly EFDbContext context = new EFDbContext();
        
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }
}
