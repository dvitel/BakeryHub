using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryHub.Domain
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public int SupplierId { get; set; }        
        public int ProductCategoryId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableNow { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid? MainImageId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public IList<ProductImage> Images { get; set; }
        public ProductImage MainImage { get; set; }
        public IList<ProductReview> ProductReviews { get; set; }
    }
}
