using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain.Models
{
    public class Supplier
    {
        public long Id { get; set; }
        public string Name{ get; set; }
        public string Address { get; set;  }
        public Product[] Products { get; set; }
    }
}
