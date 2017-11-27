using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    //finished
    public class CartItem
    {
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public DateTime DatePlaced { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductCount { get; set; }
    }
}
