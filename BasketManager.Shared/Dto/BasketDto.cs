using System;
using System.Collections.Generic;

namespace BasketManager.Dto
{
    [Serializable]
    public class BasketDto
    {
        public List<ProductDto> Products { get; set; }
    }
}
