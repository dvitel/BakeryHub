using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Handshake
    {
        public enum HanshakeTurn { Customer, Supplier };
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }       
        public DateTime TimeStamp { get; set; }
        public HanshakeTurn Turn { get; set; }
        public Order Order { get; set; }
        public IList<HandshakeComment> Comments { get; set; }
    }
}
