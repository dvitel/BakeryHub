using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain.Models
{
    class Delivery
    {
        public enum DeliveryState { InProgress, Shipped, Delivered }
        public int OrderId { get; set; } //what
        public int SupplierId { get; set; } //from where
        public int SupplierAddressId { get; set; } 
        //public int SupplierContactId { get; set; }
        public int CustomerId { get; set; } //to where
        public int CustomerAddressId { get; set; }
        public int? DeliverySiteId { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public Order Order { get; set; }
        public Supplier Supplier { get; set; }
        public Customer Customer { get; set; }
        public DeliverySite DeliverySite { get; set; }
    }
}
