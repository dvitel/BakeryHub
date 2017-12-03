using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }    
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCanceled { get; set; }
        public int SeqNumInOrer { get; set; }
    }
}
