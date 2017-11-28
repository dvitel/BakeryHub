using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Models
{
    public class RegisterData
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("RepeatPassword", ErrorMessage = "Password and its repeat do not match")]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string RepeatPassword { get; set; }
    }
}
