using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ProductReview
    {
        public Guid ReviewId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public Review Review { get; set; }

    }
}
