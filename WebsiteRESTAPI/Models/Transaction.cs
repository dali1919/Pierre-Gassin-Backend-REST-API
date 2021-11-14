using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Helper;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "Transaction")]
    public class Transaction
    {
        private string _extendedData;
        [Key]
        public long TransactionId { get; set; }

        [ForeignKey("Email")]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ImageId { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ImageSeizes { get; set; }
        [Column(TypeName = "json")]
        [NotMapped]
        public JObject Devis
        {
            get
            {
                return JsonConvert.DeserializeObject<JObject>(string.IsNullOrEmpty(_extendedData) ? "{}" : _extendedData);
            }
            set
            {
                _extendedData = value.ToString();
            }
        }
    }
}
