using System;
using System.Collections.Generic;

namespace Models
{
    public class Order : Entity
    {
        public DateTime DateTime { get; set; }
        public string Currency { get; set; }
        public decimal Cost { get; set; }
        public ICollection<Product> Products { get; set; } 
    }
}
