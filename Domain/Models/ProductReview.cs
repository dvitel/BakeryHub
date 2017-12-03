using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ProductReview
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Review Review { get; set; }
        public Product Product { get; set; }

    }
}
