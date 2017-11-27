using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Order
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime DatePlaced { get; set; }        
        public IList<OrderItem> OrderItems { get; set; }
        public decimal Price { get; set; }
        public Customer Customer { get; set; }
    }
}
