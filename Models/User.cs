using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : Entity
    {
        [Required]
        public string Username { get; set; }
        [StringLength(maximumLength: 32, MinimumLength = 8)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
