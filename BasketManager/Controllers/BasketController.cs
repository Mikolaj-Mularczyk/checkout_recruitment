
using System;
using System.Collections.Generic;
using System.Linq;

using BasketManager.Dto;
using BasketManager.Services;
using BasketManager.Shared.Dto;

using Microsoft.AspNetCore.Mvc;

namespace BasketManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        private readonly IProductRepository productRepository;

        public BasketController(IBasketService basketService, IProductRepository productRepository)
        {
            this.basketService = basketService;
            this.productRepository = productRepository;
        }

        [HttpGet(nameof(GetBasket))]
        public BasketDto GetBasket(Guid userId)
        {
            var basket = basketService.GetBasket(userId);
            if (basket == null) return null;
            var products = productRepository.GetProducts(basket.Products.Select(o => o.Key));
            var productDtos = products.Select(
                o => new ProductDto
                {
                    Category = (int)o.Category,
                    Name = o.Name,
                    ProductId = o.ProductId,
                    Quantity = basket.Products[o.ProductId]
                }).ToList();
            return new BasketDto { Products = productDtos };
        }

        [HttpPut(nameof(AddOrUpdateProduct))]
        public IActionResult AddOrUpdateProduct(
            [FromBody] AddOrUpdateRequestBody requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            basketService.AddOrUpdateProduct(requestBody.ProductId, requestBody.Quantity, requestBody.UserId);
            return Ok();
        }

        [HttpDelete(nameof(RemoveProduct))]
        public IActionResult RemoveProduct(Guid productId, Guid userId)
        {
            try
            {
                basketService.RemoveProduct(productId, userId);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete(nameof(ClearBasket))]
        public IActionResult ClearBasket(Guid userId)
        {
            try
            {
                basketService.ClearBasket(userId);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
