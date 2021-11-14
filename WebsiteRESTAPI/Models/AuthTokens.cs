using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "AuthTokens")]
    public class AuthTokens
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Token { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string UserCode { get; set; }
    }
}
