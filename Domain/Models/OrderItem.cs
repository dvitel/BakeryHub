using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class OrderItem
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }        
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCanceled { get; set; }
    }
}
