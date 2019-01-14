using System;
using System.Collections.Generic;

using BasketManager.Models;

namespace BasketManager.Services
{
    class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IProductRepository productRepository;

        public BasketService(IBasketRepository basketRepository, IProductRepository productRepository)
        {
            this.basketRepository = basketRepository;
            this.productRepository = productRepository;
        }

        public Basket GetBasket(Guid userId) => basketRepository.GetBasket(userId);

        public void AddOrUpdateProduct(Guid productId, int quantity, Guid userId)
        {
            if (!productRepository.ContainsProduct(productId))
                throw new KeyNotFoundException("No such product.");

            var basket = basketRepository.GetBasket(userId);
            if (basket == null)
            {
                basket = new Basket();
                basketRepository.AddOrUpdateBasket(userId, basket);
            }


            if (basket.Products.ContainsKey(productId))
                basket.Products[productId] = quantity;
            else
                basket.Products.Add(productId, quantity);

            basketRepository.AddOrUpdateBasket(userId, basket);
        }

        public void RemoveProduct(Guid productId, Guid userId)
        {
            var basket = basketRepository.GetBasket(userId);
            if (basket == null)
            {
                throw new KeyNotFoundException($"No basket for user with userId: {userId}");
            }

            if (!basket.Products.ContainsKey(productId))
                throw new KeyNotFoundException(
                    $"No product with {productId} for user "); // product name instead of guid?
            basket.Products.Remove(productId);

            basketRepository.AddOrUpdateBasket(userId, basket);
        }

        public void ClearBasket(Guid userId)
        {
            var basket = basketRepository.GetBasket(userId);
            if (basket == null)
            {
                throw new KeyNotFoundException($"No basket for user with userId: {userId}");
            }

            if (!basketRepository.RemoveBasket(userId))
                throw new InvalidOperationException($"Something went wrong when removing basket for user: {userId}");
        }
    }
}
