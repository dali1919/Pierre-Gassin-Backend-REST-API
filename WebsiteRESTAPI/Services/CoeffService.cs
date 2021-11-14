using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Services
{
    public class CoeffService : ICoeff
    {
        /// <summary>
        /// Get the price coefficients
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetOne(ApplicationDbContext dbContext)
        {
            try
            {
                var coeff = dbContext.Coeffs.FirstOrDefault();
                if (coeff != null)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(coeff)
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);
                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = " There is no data found"
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.NotFound);
                }

            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong"
                };
                return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.InternalServerError);

            }
        }
        /// <summary>
        /// Update the price coefficients
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="id"></param>
        /// <param name="coeff"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> UpdateCoeffById(ApplicationDbContext dbContext,long id, Coeff coeff)
        {
            try
            {
                var mycoeff = dbContext.Coeffs.FirstOrDefault(x => x.Id == id);
                if(mycoeff!=null)
                {
                    dbContext.Coeffs.Update(coeff);
                    dbContext.SaveChanges();
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(coeff)
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    dbContext.Coeffs.Add(coeff);
                    dbContext.SaveChanges();
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(coeff)
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }


            }
            catch (Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong"
                };
                return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.InternalServerError);

            }
        }
    }
}
