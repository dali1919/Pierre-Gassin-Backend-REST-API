using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;
using WebsiteRESTAPI.ViewModels;

namespace WebsiteRESTAPI.Interface
{
    public interface IReportage
    {
        Task<APIResponseResult<object>> GetAll(ApplicationDbContext dbContext);
        Task<APIResponseResult<object>> GetReportageByID(ApplicationDbContext dbContext,long Id);
        Task<APIResponseResult<object>> GetReportageBySlug(ApplicationDbContext dbContext, string Slug);
        Task<APIResponseResult<object>> LikeReportage(ApplicationDbContext dbContext, long Id);
        Task<APIResponseResult<object>> AddReportage(ApplicationDbContext dbContext, RepoDescription repo);
        Task<APIResponseResult<object>> AddPhoto(ApplicationDbContext dbContext, IFormFile tagged, IFormFile size1, IFormFile size2, IFormFile size3, long id);
        Task<APIResponseResult<object>> GetPhotoById(ApplicationDbContext dbContext, long id);
        Task<APIResponseResult<object>> DeletePhoto(ApplicationDbContext dbContext, long Id);
        Task<APIResponseResult<object>> DeleteReportage(ApplicationDbContext dbContext, long Id);
        Task<APIResponseResult<object>> UpdateReportage(ApplicationDbContext dbContext, long Id, string Title, string Description);
    }
}
