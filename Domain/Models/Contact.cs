using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Contact
    {
        public enum ContactType { Email, Phone, Mobile };
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } //email, phone n        
        public ContactType Type { get; set; }
        public bool IsConfirmed { get; set; }   
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public IList<OrderSubscription> OrderSubscriptions { get; set; }
        public IList<FeedbackSubscription> FeedbackSubscriptions { get; set; }
        public IList<ReportSubscription> ReportSubscriptions { get; set; }
        public IList<NotificationLog> Sendings { get; set; }
    }
}
