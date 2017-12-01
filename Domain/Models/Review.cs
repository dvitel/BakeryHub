using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int TargetUserId { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }
        public bool IsAboutProduct { get; set; }
    }
}
