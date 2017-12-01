﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }        
        public IList<Order> Orders { get; set; }        
    }
}
