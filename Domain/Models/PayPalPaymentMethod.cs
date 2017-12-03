using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class PayPalPaymentMethod
    {
        public Guid PaymentMethodId { get; set; }   
        public string PayPalAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
