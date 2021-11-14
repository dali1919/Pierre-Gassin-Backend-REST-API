using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "User")]
    public class User
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Column(TypeName = "varchar(255)")]
        public string Password { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [Column(TypeName = "varchar(255)")]
        public string UserName { get; set; }


        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Company { get; set; }
        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Tva { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Adress { get; set; }
        [Column(TypeName = "int")]
        public int ZipCode { get; set; }
        [Column(TypeName = "int")]
        public int Phone { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Appartement { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Region { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
