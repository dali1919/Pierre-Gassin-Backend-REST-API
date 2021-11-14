using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Controllers
{
    [Authorize]
    [Route("/[controller]/")]
    //[ApiController]
    public class TransactionController : Controller
    {
        private ApplicationDbContext dataContext;
        private readonly ITransaction itransaction;
        public TransactionController(ApplicationDbContext context, ITransaction transaction)
        {
            dataContext = context;
            itransaction = transaction;
        }
        [HttpPost("")]
        public async Task<IActionResult> Checkout([FromBody] Payment payment )
        {
            try
            {
                var result = await itransaction.AddPayment(dataContext, payment);
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
        [HttpGet("")]
        public async Task<IActionResult> GetTransactionById(string email)
        {
            try
            {
                var result = await itransaction.GetTransactionsByAccount(dataContext, email);
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
