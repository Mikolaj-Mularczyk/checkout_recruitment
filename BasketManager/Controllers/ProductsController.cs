using System;
using System.Linq;

using BasketManager.Dto;
using BasketManager.Models;
using BasketManager.Services;
using BasketManager.Shared.Dto;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy = "ApiUser", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet(nameof(GetProducts))]
        public ActionResult<Product[]> GetProducts([FromQuery]GetProductsRequestBody requestBody)
        {
            var parsingSuccessful = Enum.TryParse(requestBody.Category, true, out ProductCategory parsedCategory);
            if (!parsingSuccessful)
                return BadRequest("No such category.");
            return productRepository.GetProducts(parsedCategory)
                .Skip((requestBody.PageNumber - 1) * requestBody.PageSize).Take(requestBody.PageSize).ToArray();
        }

        [HttpGet(nameof(GetCategories))]
        public ActionResult<CategoryDto[]> GetCategories()
        {
            var enumValues = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>();
            return enumValues.Select(o => new CategoryDto() { Name = o.ToString(), Value = (int)o }).ToArray();
        }
    }
}
