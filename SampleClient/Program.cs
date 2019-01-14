using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MyNamespace;

namespace SampleClient
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();

        const string baseUrl = "https://localhost:44369/";

        static async Task Main(string[] args)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var authClient = new AuthClient(baseUrl, httpClient);
            var basketClient = new BasketClient(baseUrl, httpClient);
            var productsClient = new ProductsClient(baseUrl, httpClient);

            var response = await authClient.LoginAsync();
            string responseContent;
            using (var sr = new StreamReader(response.Stream))
            {
                responseContent = sr.ReadToEnd();
            }

            var match = Regex.Match(responseContent, @"(?<=bearerToken\\"": \\"")(.*)(?=\\"",)").Value;
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + match);

            var categories = await productsClient.GetCategoriesAsync();
            var category = categories.First();
            var lastCategory = categories.Last();

            var productsFirstCategory = await productsClient.GetProductsAsync(category.Name, 10, 1);
            var productsLastCategory = await productsClient.GetProductsAsync(lastCategory.Name, 1, 2);
            var product = productsFirstCategory.First();

            var userId = Guid.NewGuid();
            await basketClient.AddOrUpdateProductAsync(
                new AddOrUpdateRequestBody { ProductId = product.ProductId, Quantity = 2, UserId = userId });

            var basket = await basketClient.GetBasketAsync(userId);

            await basketClient.ClearBasketAsync(userId);

            basket = await basketClient.GetBasketAsync(userId);

            var product2 = productsLastCategory.Last();
            await basketClient.AddOrUpdateProductAsync(
                new AddOrUpdateRequestBody { ProductId = product2.ProductId, Quantity = 3, UserId = userId });

            await basketClient.AddOrUpdateProductAsync(
                new AddOrUpdateRequestBody { ProductId = product2.ProductId, Quantity = 1, UserId = userId });

            await basketClient.AddOrUpdateProductAsync(
                new AddOrUpdateRequestBody { ProductId = product.ProductId, Quantity = 1, UserId = userId });

            try
            {
                await basketClient.AddOrUpdateProductAsync(
                    new AddOrUpdateRequestBody { ProductId = product.ProductId, Quantity = -2, UserId = userId });
            }
            catch (Exception)
            { }

            basket = await basketClient.GetBasketAsync(userId);

            await basketClient.RemoveProductAsync(product2.ProductId, userId);

            basket = await basketClient.GetBasketAsync(userId);

            await basketClient.ClearBasketAsync(userId);

            basket = await basketClient.GetBasketAsync(userId);
        }
    }
}
