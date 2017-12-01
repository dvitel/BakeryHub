using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CardPaymentMethod
    {
        public int UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int BillingAddressId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Address BillingAddress { get; set; }
    }
}
