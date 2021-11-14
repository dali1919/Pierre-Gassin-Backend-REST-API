using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Controllers
{
    [Route("/blog/")]
    public class BlogController : Controller
    {
        private ApplicationDbContext dataContext;
        private IBlog IBlog;
        public BlogController(ApplicationDbContext context ,IBlog blog)
        {
            dataContext = context;
            IBlog = blog;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await IBlog.GetAll(dataContext);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogByID(int id)
        {
            try
            {
                var result = await IBlog.GetBlogByID(dataContext,id);
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
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBlogByID(string slug)
        {
            try
            {
                var result = await IBlog.GetBlogBySlug(dataContext, slug);
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
        public async Task<IActionResult> UpdateBlogByID(IFormFile cover, [FromForm] string title, [FromForm] string description, long id)
        {
            try
            {
                var result = await IBlog.UpdateBlog(dataContext, cover,title,description,id);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> UpdateBlogByID(long id)
        {
            try
            {
                var result = await IBlog.DeleteBlog(dataContext, id);
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
        [HttpPost("")]
        public async Task<IActionResult> AddBlog( IFormFile cover, [FromForm] string title, [FromForm] string description)
        {
            try
            {
                var result = await IBlog.AddBlog(dataContext,cover,title,description);
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
