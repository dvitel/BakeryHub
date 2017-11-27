using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CustomerContactSubscription
    {
        public enum ContactPurpose { Delivery, Negotiation, ProductInStock, Feedback }
        public int CustomerId { get; set; }
        public int ContactId { get; set; }
        public ContactPurpose Purpose { get; set; }
    }
}
