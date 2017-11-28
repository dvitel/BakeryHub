﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class SupplierReview
    {
        public int CustomerId { get; set; }
        public int SupplierId  { get; set; }
        public DateTime Date { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }
        public Customer Customer { get; set; }
        public Supplier Supplier { get; set; }
    }
}