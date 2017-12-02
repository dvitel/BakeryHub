using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public bool HasLogo { get; set; }
        public bool IsCompany { get; set; } 
        public bool IsApproved { get; set; }
        public User User { get; set; }
        public IList<Product> Products { get; set; }        
        public IList<Order> Orders { get; set; }
    }
}
