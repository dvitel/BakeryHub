using BakeryHub.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Models
{
    public class ProductViewModel
    {
        public IList<IFormFile> Images { get; set; } //0 is main
        public IList<string> ImagePathes { get; set; }
        public int? ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableInStore { get; set; }
        public int CategoryId { get; set; }
        public IList<ProductCategory> Categories { get; set; }

    }
}
