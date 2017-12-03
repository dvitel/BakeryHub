using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class OrderSubscription
    {
        public Guid ContactId { get; set; }
        public Guid OrderId { get; set; }
        public Contact Contact { get; set; }
        public Order Order { get; set; }
    }
}
