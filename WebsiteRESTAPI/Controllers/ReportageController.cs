using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;
using WebsiteRESTAPI.Services;
using WebsiteRESTAPI.ViewModels;

namespace WebsiteRESTAPI.Controllers
{
    [Authorize]
    [Route("/reportage/")]
    //[ApiController]
    public class ReportageController : Controller
    {
        private readonly IReportage _ireportage;
        private ApplicationDbContext dataContext;
        public ReportageController(ApplicationDbContext _dataContext, IReportage reportage)
        {
            _ireportage = reportage;
            dataContext = _dataContext;

        }
  
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _ireportage.GetAll(dataContext);
                switch (result.Status)
                {
                    case HttpStatusCode.OK:
                        return this.Ok(result.Entity);
                    case HttpStatusCode.NoContent:
                        return this.NoContent();
                    case HttpStatusCode.NotFound:
                        return this.NotFound(result.Entity);
                    case HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError,result.Entity);
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
       
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetReportageByID(long id)
        {
            try
            {
                var result = await _ireportage.GetReportageByID(dataContext,id);
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
        
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetReportageBySlug(string slug)
        {
            try
            {
                var result = await _ireportage.GetReportageBySlug(dataContext, slug);
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
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> LikeReportage(long id)
        {
            try
            {
                var result = await _ireportage.LikeReportage(dataContext, id);
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
        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddReportage([FromBody] RepoDescription repo)
        {
            try
            {
                var result = await _ireportage.AddReportage(dataContext, repo);
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
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/image")]
        public async Task<IActionResult> AddPhoto(IFormFile tagged, IFormFile size1, IFormFile size2, IFormFile size3, long id)
        {
            try
            {
                var result = await _ireportage.AddPhoto(dataContext, tagged,size1,size2,size3,id);
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
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetPhotById(long id)
        {
            try
            {
                var result = await _ireportage.GetPhotoById(dataContext, id);
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeletePhoto(long id)
        {
            try
            {
                var result = await _ireportage.DeletePhoto(dataContext,id);
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportage(long id)
        {
            try
            {
                var result = await _ireportage.DeleteReportage(dataContext,id);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReportage(long id, [FromBody] string _Title, [FromBody] string _Description)
        {
            try
            {
                var result = await _ireportage.UpdateReportage(dataContext,id, _Title, _Description);
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
