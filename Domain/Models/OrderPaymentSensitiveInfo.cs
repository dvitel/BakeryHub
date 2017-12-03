using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    //trigger should delete the row when order goes from Initial to any other state
    public class OrderPaymentSensitiveInfo
    {
        public Guid OrderId { get; set; }
        public Guid CardPaymentMethodId { get; set; }
        public string CVV { get; set; }
        public string ExpirationDate { get; set; }
        public Order Order { get; set; }
        public CardPaymentMethod CardPaymentMethod { get; set; }
    }
}
