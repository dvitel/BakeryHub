using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CardPaymentMethod
    {
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public CountryState State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }
}
