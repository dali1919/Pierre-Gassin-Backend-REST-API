using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "Blog")]
    public class Blog
    {
        [Key]
        [Column(TypeName = "bigint")]
        public long BlogId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Cover { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Slug { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime date { get; set; }
    }
}
