using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Controllers
{
    [Route("/coeff/")]
    public class CoeffController : Controller
    {
        private readonly ICoeff _icoeff;
        private ApplicationDbContext _dataContext;
        public CoeffController(ApplicationDbContext dataContext, ICoeff coeff)
        {
             _icoeff = coeff;
            _dataContext = dataContext;
            ApiHelper.InitializeCoeff(_dataContext);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetOne()
        {
            try
            {
                var result = await _icoeff.GetOne(_dataContext);
                switch (result.Status)
                {
                    case HttpStatusCode.OK:
                        return this.Ok(result.Entity);
                    case HttpStatusCode.NoContent:
                        return this.NoContent();
                    case HttpStatusCode.NotFound:
                        return this.NotFound(result.Entity);
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, result.Entity);
                    case HttpStatusCode.ServiceUnavailable:
                        return this.StatusCode((int)HttpStatusCode.ServiceUnavailable, result.Entity);
                    case HttpStatusCode.BadRequest:
                        return this.StatusCode((int)HttpStatusCode.BadRequest);
                    default:
                        throw new UnhandledRepositoryActionStatusException();
                }

            }
            catch (Exception Ex0)
            {
                return Problem(Ex0.Message, "", (int)HttpStatusCode.InternalServerError);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoeff(long id, [FromBody] Coeff coeff)
        {
            try
            {
                var result = await _icoeff.UpdateCoeffById(_dataContext,id,coeff);
                switch (result.Status)
                {
                    case HttpStatusCode.OK:
                        return this.Ok(result.Entity);
                    case HttpStatusCode.NoContent:
                        return this.NoContent();
                    case HttpStatusCode.NotFound:
                        return this.NotFound(result.Entity);
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, result.Entity);
                    case HttpStatusCode.ServiceUnavailable:
                        return this.StatusCode((int)HttpStatusCode.ServiceUnavailable, result.Entity);
                    case HttpStatusCode.BadRequest:
                        return this.StatusCode((int)HttpStatusCode.BadRequest);
                    default:
                        throw new UnhandledRepositoryActionStatusException();
                }

            }
            catch (Exception Ex0)
            {
                return Problem(Ex0.Message, "", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
