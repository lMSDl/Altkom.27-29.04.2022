using System;

namespace Models
{
    public class Order : Entity
    {
        public DateTime DateTime { get; set; }
        public string Currency { get; set; }
        public decimal Cost { get; set; }
    }
}
