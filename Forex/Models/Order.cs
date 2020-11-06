using System;

namespace Forex.Models
{
    public class Order
    {
        public int OrderNo { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
