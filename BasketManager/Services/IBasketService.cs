using System;

using BasketManager.Models;

namespace BasketManager.Services
{
    public interface IBasketService
    {
        Basket GetBasket(Guid userId);

        void AddOrUpdateProduct(Guid productId, int quantity, Guid userId);

        void RemoveProduct(Guid productId, Guid userId);

        void ClearBasket(Guid userId);
    }
}
