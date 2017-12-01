using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryHub.Domain
{
    public class Product
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableNow { get; set; }
        public DateTime LastUpdated { get; set; }
        public ProductCategory Category { get; set; }
        public IList<ProductImage> Images { get; set; }
        public IList<ProductReview> ProductReviews { get; set; }
    }
}
