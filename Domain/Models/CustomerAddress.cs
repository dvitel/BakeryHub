using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class CustomerAddress
    {
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public CountryState State { get; set; }
        //public string Country { get; set; }
        public string Zip { get; set; }
    }
}
