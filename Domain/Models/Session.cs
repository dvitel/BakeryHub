using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Session
    {
        public Guid Id { get; set; }
        public DateTime FirstVisit { get; set; }
        public DateTime LastVisit { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public IList<CartItem> CartItems { get; set; }
    }
}
