using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public IList<CartItem> CartItems { get; set; }
        public IList<SupplierReview> SupplierReviews { get; set; }
        public IList<ProductReview> ProductReviews { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<Payment> Payments { get; set; }
        public IList<CustomerAddress> Addresses { get; set;}
        public IList<CustomerContact> Contacts { get; set; }
        public IList<CardPaymentMethod> PaymentMethod { get; set; }
    }
}
