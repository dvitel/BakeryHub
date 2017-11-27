using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Payment
    {
        public enum PaymentStatus { Started, Success, Fail }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int CardPaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public Customer Customer { get; set; }
        public Order Order { get; set; }
    }
}
