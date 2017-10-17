using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryHub.Domain.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public Delivery[] Delivery { get; set; }

    }
}
