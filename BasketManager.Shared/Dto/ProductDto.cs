using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BasketManager.Dto
{
    /// <summary>
    /// The product class.
    /// </summary>
    [Serializable]
    public class ProductDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }
    }
}
