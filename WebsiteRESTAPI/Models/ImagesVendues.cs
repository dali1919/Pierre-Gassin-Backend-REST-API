using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    [Keyless]
    [Table(name: "ImagesVendues")]
    public class ImagesVendues
    {
        
        [ForeignKey("TransactionId")]
        public int TransactionId { get; set; }

        [ForeignKey("ImageId")]
        public int ImageId { get; set; }
    }
}
