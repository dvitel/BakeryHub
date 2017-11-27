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
        public string Mime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsMain { get; set; }
    }
}
