using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class ReportSubscription
    {
        public enum ReportType { MonthlySales, MonthlyDeliveries, OrderAvailable, Feedback }
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public ReportType Type { get; set; }
    }
}
