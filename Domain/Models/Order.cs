using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Order
    {
        public enum OrderStatus { Initiated, Canceled, PaymentFailed, TakenToWork, Done, Shipped, Delivered }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public DateTime DatePlaced { get; set; }        
        public DateTime PlannedDeliveryDate { get; set; }        
        public decimal Price { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public Customer Customer { get; set; }
        public Supplier Supplier { get; set; }
        public IList<OrderSubscription> Subscriptions { get; set; }
    }
}
