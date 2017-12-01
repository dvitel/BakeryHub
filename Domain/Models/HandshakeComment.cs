using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class HandshakeComment
    {
        public Guid HandshakeId { get; set; }
        public int ProductId { get; set; }
        public string Comment { get; set; }
        public decimal? NewPrice { get; set; }
        public bool IsCanceled { get; set; }
    }
}
