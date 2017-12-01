using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Delivery
    {        
        public enum DeliveryStatus { Requested, Canceled, Taken, OrderShipped, OrderDelivered }
        public int CustomerId { get; set; } //to where
        public int OrderId { get; set; }        
        public int SupplierId { get; set; } //from where
        public int DeliverySiteId { get; set; }
        public int SupplierAddressId { get; set; } 
        //public int SupplierContactId { get; set; }        
        public int CustomerAddressId { get; set; } 
        public decimal? Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public DeliveryStatus Status { get; set; }
        public Order Order { get; set; }
        public Address CustomerAddress { get; set; }
        public Address SupplierAddress { get; set; }
    }
}
