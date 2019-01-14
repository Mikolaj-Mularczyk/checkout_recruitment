using System;
using System.Collections.Concurrent;
using BasketManager.Models;

namespace BasketManager.Services
{
    public class BasketRepository : IBasketRepository
    {
        static readonly ConcurrentDictionary<Guid, Basket> Baskets = new ConcurrentDictionary<Guid, Basket>();

        public void AddOrUpdateBasket(Guid userGuid, Basket basket) =>
            Baskets.AddOrUpdate(userGuid, basket, (guid, _) => Baskets[guid] = basket);

        public Basket GetBasket(Guid userGuid) =>
            Baskets.TryGetValue(userGuid, out var basket) ? basket : null;

        public bool RemoveBasket(Guid userGuid) => Baskets.TryRemove(userGuid, out _);
    }
}
