using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Delivery
    {        
        public enum DeliveryStatus { Waiting, Requested, Canceled, Taken }
        public Guid DeliveryId { get; set; }
        public Guid OrderId { get; set; }        
        public int? DeliverySiteId { get; set; }
        public Guid SupplierAddressId { get; set; } 
        //public int SupplierContactId { get; set; }        
        public Guid CustomerAddressId { get; set; } 
        public decimal? Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public DeliveryStatus Status { get; set; }
        public Order Order { get; set; }
        public DeliverySite DeliverySite { get; set; }
        public Address CustomerAddress { get; set; }
        public Address SupplierAddress { get; set; }
    }
}
