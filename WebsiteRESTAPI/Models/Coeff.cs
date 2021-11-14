using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Models
{
    public class Coeff
    {
        [Key]
        public long Id { get; set; }
        public double Print { get; set; }
        public double Web { get; set; }
        public double Print_web { get; set; }
        public double National{ get; set; }
        public double Europe { get; set; }
        public double Mondial { get; set; }
        public double Double { get; set; }
        public double Couverture { get; set; }
        public double Pleine { get; set; }
        public double Demi { get; set; }
        public double Quart { get; set; }
        public double n1000 { get; set; }
        public double n10000 { get; set; }
        public double n100000 { get; set; }
        public double ns100000 { get; set; }

    }
}
