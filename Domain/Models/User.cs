using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class User
    {
        public enum PasswordEncryption { Plain, MD5, DES }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public PasswordEncryption PasswordEncryptionAlgorithm { get; set; }
        public string Salt { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public Customer Customer { get; set; }
        public Supplier Supplier { get; set; }
        public DeliverySite DeliverySite { get; set; }
        public IList<Address> Addresses { get; set; }
        public IList<Contact> Contacts { get; set; }
        public IList<Payment> Payments { get; set; }
        public IList<Payment> ReceivedPayments { get; set; }
        public IList<PaymentMethod> PaymentMethods { get; set; }
        public IList<CardPaymentMethod> CardPaymentMethod { get; set; }
        public IList<PayPalPaymentMethod> PayPalPaymentMethod { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<Review> ReceivedReviews { get; set; }

    }
}
