using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class PaymentMethod
    {
        public enum PaymentMethodType { Card, PayPal }
        public Guid PaymentMethodId { get; set; }
        public int UserId { get; set; }        
        public string UIDesc { get; set; }
        public PaymentMethodType Type { get; set; }
        public bool IsDeleted { get; set; }
        public IList<Payment> Payments { get; set; }
    }
}
