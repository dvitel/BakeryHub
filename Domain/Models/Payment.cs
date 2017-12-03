using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Payment
    {
        public enum PaymentTarget { Order, Delivery }
        public enum PaymentStatus { Started, Success, Fail }
        public Guid PaymentId { get; set; }
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public Guid OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentTarget Target { get; set; }
        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
