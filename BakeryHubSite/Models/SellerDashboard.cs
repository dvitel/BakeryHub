using BakeryHub.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Models
{
    public class SellerDashboard
    {
        public IList<Product> Products { get; set; }
        public IList<Order> Orders { get; set; } 
    }
}
