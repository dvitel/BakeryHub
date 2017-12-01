using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    //finished
    public class CartItem
    {
        public Guid SessionId { get; set; }
        public int ItemId { get; set; }
        public DateTime DatePlaced { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }                
        public int ProductCount { get; set; }
        public Product Product { get; set; }
    }
}
