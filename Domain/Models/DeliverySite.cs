using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain.Models
{
    public class DeliverySite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isCompany { get; set; }
    }
}
