using System;

using BasketManager.Models;

namespace BasketManager.Services
{
    public interface IBasketRepository
    {
        void AddOrUpdateBasket(Guid userGuid, Basket basket);

        Basket GetBasket(Guid userGuid);

        bool RemoveBasket(Guid userGuid);
    }
}
