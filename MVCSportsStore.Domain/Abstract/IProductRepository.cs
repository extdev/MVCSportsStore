using System.Collections.Generic;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
