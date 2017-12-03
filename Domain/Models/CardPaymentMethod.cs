using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CardPaymentMethod
    {
        public Guid PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public Guid BillingAddressId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Address BillingAddress { get; set; }
    }
}
