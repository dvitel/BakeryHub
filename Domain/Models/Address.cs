using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public int UserId { get; set; }        
        public string Street { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string CountryId { get; set; }
        public bool IsBilling { get; set; }
        public bool IsDeleted { get; set; }
        public string Zip { get; set; }
        public CountryState State { get; set; }
        public Country Country { get; set; }
    }
}
