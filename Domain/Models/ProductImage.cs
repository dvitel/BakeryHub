using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }        
        public string Path { get; set; }
        public string LogicalPath { get; set; }
        public string Mime { get; set; }
    }
}
