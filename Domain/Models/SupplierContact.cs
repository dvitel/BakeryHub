using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class SupplierContact
    {
        public int SupplierId { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } //email, phone n
        public ContactType Type { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsUIVisible { get; set; }
        public IList<SupplierContactSubscription> Subscriptions { get; set; }
    }
}
