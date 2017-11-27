﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Product> Products { get; set; }
    }
}
