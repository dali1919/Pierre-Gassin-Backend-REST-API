using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "Image")]
    public class Image
    {
        [Key]
        public long ImageId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Tagged { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Main { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Taille1 { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Taille2 { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Taille3 { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }
        [ForeignKey("ReportageId")]
        public long ReportageId { get; set; }
    }
}
