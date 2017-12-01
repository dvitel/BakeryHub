using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    //trigger should delete the row when order goes from Initial to any other state
    public class OrderPaymentSensitiveInfo
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public string CVV { get; set; }
        public string ExpirationDate { get; set; }
        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
