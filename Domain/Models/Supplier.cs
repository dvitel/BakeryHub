using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class Supplier
    {
        //public enum SupplierKind { PrivatePerson, Company }
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public bool HasLogo { get; set; }
        public bool IsCompany { get; set; } 
        public User User { get; set; }
        public IList<SupplierAddress> Addresses { get; set; }
        public IList<SupplierContact> Contacts { get; set; }
        public IList<Product> Products { get; set; }
        public IList<SupplierReview> Reviews { get; set; }
        //public Product[] Products { get; set; }
    }
}
