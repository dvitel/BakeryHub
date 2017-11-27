using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CustomerContact
    {
        public int CustomerId { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } //email, phone n
        public ContactType Type { get; set; }
        public bool IsConfirmed { get; set; }   
        public IList<CustomerContactSubscription> Subscriptions { get; set; }
    }
}
