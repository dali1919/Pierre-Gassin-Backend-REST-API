using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Models;

namespace WebsiteRESTAPI.Entity
{
    public class Transaction
    {
        public User User { get; set; }
        public List<Image> Images { get; set; }

        public object Payment { get; set; }
        public string Status { get; set; }
    }
    public class Payment
    {
        public long Amount { get; set; }
        public string Id { get; set; }
        public User User { get; set; }
        public Dictionary<long,List<string>> Images { get; set; }
        public Dictionary<string,string> Metadata { get; set; }
       
    }
}
