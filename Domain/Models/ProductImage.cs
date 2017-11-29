using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ProductImage
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int ImageId { get; set; }
        public string Path { get; set; }
        public string LogicalPath { get; set; }
        public string Mime { get; set; }
    }
}
