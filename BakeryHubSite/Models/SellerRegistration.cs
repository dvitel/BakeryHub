using BakeryHub.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Models
{
    public class SellerContact
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } //email, phone n
    }
    public class SellerRegistration
    {
        public IList<CountryState> States { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        public IFormFile Logo { get; set; }

        public bool IsCompany { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        public string StateId { get; set; }
        public string Zip { get; set; }
        public IList<SellerContact> Contacts { get; set; }
    }
}
