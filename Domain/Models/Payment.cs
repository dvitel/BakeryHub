using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Payment
    {
        public enum PaymentTarget { Order, Delivery }
        public enum PaymentStatus { Started, Success, Fail }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public int TargetUserId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentTarget Target { get; set; }
        public Order Order { get; set; }
    }
}
