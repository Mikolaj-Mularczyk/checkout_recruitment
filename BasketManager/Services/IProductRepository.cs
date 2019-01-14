using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BasketManager.Models;

namespace BasketManager.Services
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(ProductCategory category);

        Product GetProduct(Guid productId);

        IEnumerable<Product> GetProducts(IEnumerable<Guid> productIds);

        bool ContainsProduct(Guid productId);
    }
}
