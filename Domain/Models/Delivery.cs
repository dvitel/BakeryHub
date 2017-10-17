using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public Product[] Products { get; set; }
    }
}
