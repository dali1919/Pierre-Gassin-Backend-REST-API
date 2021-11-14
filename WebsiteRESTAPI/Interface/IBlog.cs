using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Interface
{
    public interface IBlog
    {
        Task<APIResponseResult<object>> GetAll(ApplicationDbContext dbContext);
        Task<APIResponseResult<object>> GetBlogByID(ApplicationDbContext dbContext, long Id);
        Task<APIResponseResult<object>> GetBlogBySlug(ApplicationDbContext dbContext, string slug);
        Task<APIResponseResult<object>> UpdateBlog(ApplicationDbContext dbContext, IFormFile cover, string title, string description, long id);
        Task<APIResponseResult<object>> AddBlog(ApplicationDbContext dataContext, IFormFile cover, string title, string description);
        Task<APIResponseResult<object>> DeleteBlog(ApplicationDbContext dataContext, long id);
    }
}
