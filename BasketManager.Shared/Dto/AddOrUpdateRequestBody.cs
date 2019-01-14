using System;
using System.ComponentModel.DataAnnotations;

namespace BasketManager.Shared.Dto
{
    public class AddOrUpdateRequestBody
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }
    }
}
