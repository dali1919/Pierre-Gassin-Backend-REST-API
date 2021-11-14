using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Models;

namespace WebsiteRESTAPI.ViewModels
{
    public class ReportageViewModel
    {
        public Reportage Reportage { get; set; }
        public List<Image> Images { get; set; }
        public bool success { get; set; }
        public string message { get; set; }

    }
}
