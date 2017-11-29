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
        public Customer Customer { get; set; }
        public Supplier Supplier { get; set; }
    }
}
