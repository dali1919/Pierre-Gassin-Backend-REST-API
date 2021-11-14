using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Interface
{
    public interface ICoeff
    {
        Task<APIResponseResult<object>> GetOne(ApplicationDbContext dbContext);
        Task<APIResponseResult<object>> UpdateCoeffById(ApplicationDbContext dbContext, long id, Coeff coeff);
       
    }
}
