using System;
using System.ComponentModel.DataAnnotations;

namespace BasketManager.Shared.Dto
{
    [Serializable]
    public class GetProductsRequestBody
    {
        public string Category { get; set; }

        [Range(1, 200)]
        public int PageSize { get; set; }

        [Range(1, 1000000)]
        public int PageNumber { get; set; }
    }
}
