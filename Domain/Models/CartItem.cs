using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    //finished
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public DateTime DatePlaced { get; set; }
        public Guid ProductId { get; set; }                
        public int ProductCount { get; set; }
        public Product Product { get; set; }
    }
}
