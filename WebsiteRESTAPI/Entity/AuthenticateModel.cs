using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Entity
{
    public class AuthenticateModel
    {
        
        public string Username { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }
    }
}
