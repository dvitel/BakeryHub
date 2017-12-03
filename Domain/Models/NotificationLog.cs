using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class NotificationLog
    {
        public enum NotificationDelivery { Delivered, Failed }
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public NotificationDelivery Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
