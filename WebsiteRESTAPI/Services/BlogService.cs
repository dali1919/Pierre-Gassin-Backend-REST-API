using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Services
{
    public class BlogService : IBlog
    {

        /// <summary>
        /// Add a blog
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="cover"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> AddBlog(ApplicationDbContext dataContext, IFormFile cover, string title, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Enter the title please"
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.BadRequest);

                }
                else
                {
                    string slug = StringExtensions.Slugify(title);
                    var blog = dataContext.Blogs.SingleOrDefault(x => x.Slug == slug);
                    if (blog != null)
                    {

                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = false,
                            message = "The blog already exists"
                        };
                        return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.BadRequest);

                    }
                    else
                    {
                        string fileName = cover.FileName;
                        byte[] fileData;
                        using (var target = new MemoryStream())
                        {
                            cover.CopyTo(target);
                            fileData = target.ToArray();
                        }
                        string mimeType = cover.ContentType;
                        BlobService blobStorage = new BlobService();
                        string path = blobStorage.UploadFileToBlob($"covers/{fileName}", fileData, mimeType);
                        Blog myblog = new Blog()
                        {
                            Slug = slug,
                            date = DateTime.Now,
                            Title = title,
                            Description = description,
                            Cover = path,

                        };
                        var myaddedBlog = dataContext.Blogs.Add(myblog);
                        dataContext.SaveChanges();

                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = true,
                            data = myaddedBlog.Entity.BlogId
                        };
                        return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                    }

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
        /// <summary>
        /// Delete a blog
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> DeleteBlog(ApplicationDbContext dataContext, long id)
        {

            try
            {
                var blog = dataContext.Blogs.SingleOrDefault(x => x.BlogId == id);
                if (blog != null)
                {
                    dataContext.Blogs.Remove(blog);
                    dataContext.SaveChanges();
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "The requested blog does not exist"
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
        /// Get all blogs
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetAll(ApplicationDbContext dbContext)
        {
            try
            {
                var blogs = dbContext.Blogs.ToList();
                if (blogs.Count > 0)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(blogs),
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "There is no blog found"
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
        /// Get blog by Id
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetBlogByID(ApplicationDbContext dbContext, long Id)
        {
            try
            {
                var blog = dbContext.Blogs.SingleOrDefault(x => x.BlogId == Id);
                if (blog != null)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(blog),
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "There is no blog found"
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
        /// Get blog by slag
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetBlogBySlug(ApplicationDbContext dbContext, string slug)
        {
            try
            {
                var blog = dbContext.Blogs.SingleOrDefault(x => x.Slug == slug);
                if (blog != null)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(blog),
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "There is no blog found"
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
        /// Update an existing blog
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="cover"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> UpdateBlog(ApplicationDbContext dbContext, IFormFile cover, string title, string description, long id)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Enter the title please"
                    };
                    return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.BadRequest);

                }

                else
                {
                   if(id<=0)
                   {
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = false,
                            message = "Enter blog Id"
                        };
                        return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.BadRequest);
                   }
                   else
                    {
                        
                        string slug = StringExtensions.Slugify(title);
                        var blog = dbContext.Blogs.SingleOrDefault(x => x.BlogId == id);
                        if (blog != null)
                        {



                            string fileName = cover.FileName;
                            byte[] fileData;
                            using (var target = new MemoryStream())
                            {
                                cover.CopyTo(target);
                                fileData = target.ToArray();
                            }
                            string mimeType = cover.ContentType;
                            BlobService blobStorage = new BlobService();
                            string path = blobStorage.UploadFileToBlob($"covers/{fileName}", fileData, mimeType);
                            Blog myblog = new Blog()
                            {
                                Slug = slug,
                                date = DateTime.Now,
                                Title = title,
                                Description = description,
                                Cover = path,
                                BlogId=blog.BlogId

                            };
                            var myaddedBlog = dbContext.Blogs.Update(myblog);
                            dbContext.SaveChanges();

                            ApiResonse apiResonse = new ApiResonse()
                            {
                                success = true,
                                data = myaddedBlog.Entity.BlogId
                            };
                            return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.OK);
                        }
                        else
                        {
                            ApiResonse apiResonse = new ApiResonse()
                            {
                                success = false,
                                message = "There is no blog found"
                            };
                            return new APIResponseResult<object>(apiResonse, System.Net.HttpStatusCode.BadRequest);
                        }
                    }

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
