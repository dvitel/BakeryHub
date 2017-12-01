using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class OrderSubscription
    {
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
    }
}
