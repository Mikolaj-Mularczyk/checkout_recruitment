using System;
using System.Collections.Generic;
using System.Linq;

using BasketManager.Models;

namespace BasketManager.Services
{
    class ProductRepository : IProductRepository
    {
        static List<Product> products = new List<Product>
        {
          new Product { Category = ProductCategory.Home, Name = "Sofa", ProductId = Guid.NewGuid() },
          new Product { Category = ProductCategory.Home, Name = "Chair", ProductId = Guid.NewGuid() },
            new Product { Category = ProductCategory.Electronics, Name = "TV", ProductId = Guid.NewGuid() },
            new Product { Category = ProductCategory.Electronics, Name = "Phone", ProductId = Guid.NewGuid() },
            new Product { Category = ProductCategory.Fashion, Name = "Fancy dress", ProductId = Guid.NewGuid() },
            new Product { Category = ProductCategory.Fashion, Name = "Jacket", ProductId = Guid.NewGuid() }
        };

        public IEnumerable<Product> GetProducts(ProductCategory category)
        {
            return products.Where(o => o.Category == category);
        }

        public Product GetProduct(Guid productId)
        {
            return products.FirstOrDefault(o => o.ProductId == productId);
        }

        public IEnumerable<Product> GetProducts(IEnumerable<Guid> productIds)
        {
            return products.Where(o => productIds.Contains(o.ProductId));
        }

        public bool ContainsProduct(Guid productId)
        {
            return products.Any(o => o.ProductId == productId);
        }
    }
}
