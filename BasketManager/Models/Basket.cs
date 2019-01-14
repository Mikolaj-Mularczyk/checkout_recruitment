using System;
using System.Collections.Generic;

namespace BasketManager.Models
{
    [Serializable]
    public class Basket
    {
        public Basket()
        {
            Products = new Dictionary<Guid, int>();
        }

        public Dictionary<Guid, int> Products { get; set; }
    }
}
