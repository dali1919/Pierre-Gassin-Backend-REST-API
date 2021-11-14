using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Interface
{
    public interface ITransaction
    {
        Task<APIResponseResult<object>> AddPayment(ApplicationDbContext dbContext, Payment payment);
        Task<APIResponseResult<object>> GetTransactionsByAccount(ApplicationDbContext dbContext, string Email);
        
    }
}
