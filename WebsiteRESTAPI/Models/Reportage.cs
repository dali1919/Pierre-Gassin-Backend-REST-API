using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    [Table(name: "Reportage")]
    public class Reportage
    {
        [Key]
        public long ReportageId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }
        [Column(TypeName = "bigint")]
        public int Likes { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Slug { get; set; }        

    }
}
