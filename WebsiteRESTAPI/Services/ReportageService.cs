using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;
using WebsiteRESTAPI.ViewModels;

namespace WebsiteRESTAPI.Services
{
    public class ReportageService : IReportage
    {
        /// <summary>
        /// Add a phtoto to an exixting reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagged"></param>
        /// <param name="size1"></param>
        /// <param name="size2"></param>
        /// <param name="size3"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> AddPhoto(ApplicationDbContext dbContext, IFormFile tagged, IFormFile size1, IFormFile size2, IFormFile size3, long id)
        {
            try
            {
                if (id== 0)
                {
                    ApiResonse apiResonse1 = new ApiResonse()
                    {
                        success = false,
                        message = "Not found"
                    };
                    return new APIResponseResult<object>(apiResonse1, HttpStatusCode.NotFound);

                }
                var reportage = dbContext.Reportages.SingleOrDefault(x => x.ReportageId== id);
                if (reportage==null)
                {
                    ApiResonse apiResonse1 = new ApiResonse()
                    {
                        success = false,
                        message = "Reportage not found"
                    };
                    return new APIResponseResult<object>(apiResonse1, HttpStatusCode.NotFound);

                }

                Dictionary<string, string> paths = new Dictionary<string, string>();
                Image myImage = new Image();
                myImage.ReportageId = id;
                Dictionary<string, IFormFile> photos = new Dictionary<string, IFormFile>();
                photos.Add("tagged", tagged);
                photos.Add("size1", size1);
                photos.Add("size2", size2);
                photos.Add("size3", size3);

                foreach (var item in photos)
                {
                    string extension = item.Value.FileName.Substring(item.Value.FileName.LastIndexOf("."));
                    StringBuilder builder = new StringBuilder();
                    builder.Append(item.Value.FileName.Remove(item.Value.FileName.LastIndexOf(".")));
                    builder.Append('#');
                    builder.Append(item.Key);
                    builder.Append(extension);
                    string fName = builder.ToString();
                    string.Concat(item.Value.FileName.Remove(item.Value.FileName.LastIndexOf(".")), "#", item.Key, ".jpg");

                    byte[] fileData;
                    using (var target = new MemoryStream())
                    {
                        item.Value.CopyTo(target);
                        fileData = target.ToArray();
                    }


                    //var fileStream = new FileStream(Path.Combine(uploads, product.File.FileName), FileMode.Create);  
                    string mimeType = item.Value.ContentType;
                    //= new byte[product.File.Length];  
                    //-----------------------------------blob-------------------------------
                    BlobService blobstorage = new BlobService();
                    string path = blobstorage.UploadFileToBlob(fName, fileData, mimeType);
                    paths.Add(item.Key, path);

                }
                string taggedPhotoPath;
                if (paths.TryGetValue("tagged", out taggedPhotoPath) == true)
                    myImage.Tagged = taggedPhotoPath;
                string size1PhotoPath;
                if (paths.TryGetValue("size1", out size1PhotoPath))
                    myImage.Taille1 = size1PhotoPath;
                string size2PhotoPath;
                if (paths.TryGetValue("size2", out size2PhotoPath))
                    myImage.Taille2 = size2PhotoPath;
                string size3PhotoPath;
                if (paths.TryGetValue("size3", out size3PhotoPath))
                    myImage.Taille3 = size3PhotoPath;



                dbContext.Images.Add(myImage);
                dbContext.SaveChanges();
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = true,
                    message = "successfully created!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);

            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Add a new reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="repo"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> AddReportage(ApplicationDbContext dbContext, RepoDescription repo)
        {
            try
            {
                if (string.IsNullOrEmpty(repo.Title))
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "missing informations"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.BadRequest);
                }
                var _reportage = dbContext.Reportages.SingleOrDefault(x => x.Title == repo.Title);
                if (_reportage !=null)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Reportage already exists"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.NotFound);
                }
                else
                {
                    try
                    {
                        string slug = WebsiteRESTAPI.Helper.StringExtensions.Slugify(repo.Title);
                        Reportage myreportage = new Reportage()
                        {

                            Title = repo.Title,
                            Description = repo.Description,
                            Likes = 0,
                            Slug = slug,
                        };

                        dbContext.Reportages.Add(myreportage);
                        dbContext.SaveChanges();
                        long? id = myreportage.ReportageId;
                        string message = "The reportage is added successfully";
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = true,
                            message = "Successfully created!",
                            data= id
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                    }
                    catch (Exception ex)
                    {
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = false,
                            message = "something went wrong!"
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);

                    }
                }
            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// delete a photot from a reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> DeletePhoto(ApplicationDbContext dbContext, long Id)
        {
            try
            {
                var image = dbContext.Images.Find(Id);
                if (image != null)
                {
                    dbContext.Images.Remove(image);
                    dbContext.SaveChanges();
                    return new APIResponseResult<object>(null, HttpStatusCode.OK);
                }
                return new APIResponseResult<object>(null, HttpStatusCode.NotModified);
            }
            catch(Exception Ex)
            {
                return new APIResponseResult<object>(null, HttpStatusCode.BadRequest, Ex);
            }
        }
        /// <summary>
        /// Delete a reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> DeleteReportage(ApplicationDbContext dbContext, long Id)
        {
            try
            {
                var reportage = dbContext.Reportages.Find(Id);
                if (reportage != null)
                {
                    dbContext.Reportages.Remove(reportage);
                    dbContext.SaveChanges();
                    return new APIResponseResult<object>(null, HttpStatusCode.OK);
                }
                return new APIResponseResult<object>(null, HttpStatusCode.NotModified);

            }
            catch(Exception Ex)
            {
                return new APIResponseResult<object>(null, HttpStatusCode.BadRequest, Ex);
            }
        }
        /// <summary>
        /// Get all reportages
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetAll(ApplicationDbContext dbContext)
        {
            try
            {
                var reportages = dbContext.Reportages.ToList();
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = true,
                    data = JsonConvert.SerializeObject(reportages)
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Get a photo by id
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetPhotoById(ApplicationDbContext dbContext, long id)
        {
            try
            {
                if (id==0)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "missing informations"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.BadRequest);
                }
                var image = dbContext.Images.FirstOrDefault(x => x.ImageId == id);
                if(image!=null)
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(image)
                        
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                }
                else 
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(image)

                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.NotFound);

                }

            }
            catch(Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);

            }
        }
        /// <summary>
        /// get reportage by id
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetReportageByID(ApplicationDbContext dbContext, long Id)
        {
            try
            {
                ReportageViewModel reportageViewModel;
                HttpResponseMessage response;
                var reportage = dbContext.Reportages.Find(Id);
                if (reportage != null)
                {
                    var images = dbContext.Images.Where(x => x.ReportageId == Id).ToList<Image>();
                    if (images.Count > 0)
                    {
                        reportageViewModel = new ReportageViewModel()
                        {
                            Images = images,
                            Reportage = reportage,
                            success = true,
                            message = "The reportage is successfully loaded"
                        };
                        return new APIResponseResult<object>(reportage, HttpStatusCode.OK);
                    }
                    else
                    {
                        reportageViewModel = new ReportageViewModel()
                        {

                            Reportage = reportage,
                            success = false,
                            message = "The requested reportage doesn't contain any photo"
                        };
                        return new APIResponseResult<object>(reportageViewModel, HttpStatusCode.OK);

                    }
                }
                else
                {
                    reportageViewModel = new ReportageViewModel()
                    {


                        success = false,
                        message = "There is no reportage found"
                    };
                    return new APIResponseResult<object>(reportageViewModel, HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Get reportage by slug
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Slug"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> GetReportageBySlug(ApplicationDbContext dbContext, string Slug)
        {
            try
            {
                ReportageViewModel reportageViewModel;
                HttpResponseMessage response;
                var reportage = dbContext.Reportages.Where(x => x.Title == Slug).FirstOrDefault();
                if (reportage != null)
                {
                    var images = dbContext.Images.Where(x => x.ReportageId == reportage.ReportageId).ToList<Image>();
                    if (images.Count > 0)
                    {
                        reportageViewModel = new ReportageViewModel()
                        {
                            Images = images,
                            Reportage = reportage,
                            success = true,
                            message = "The reportage is successfully loaded"
                        };
                        return new APIResponseResult<object>(reportageViewModel, HttpStatusCode.OK);
                    }
                    else
                    {
                        reportageViewModel = new ReportageViewModel()
                        {

                            Reportage = reportage,
                            success = false,
                            message = "The requested reportage doesn't contain any photo"
                        };
                        return new APIResponseResult<object>(reportageViewModel, HttpStatusCode.OK);

                    }
                }
                else
                {
                    reportageViewModel = new ReportageViewModel()
                    {
                        success = false,
                        message = "There is no reportage found"
                    };
                    return new APIResponseResult<object>(reportageViewModel, HttpStatusCode.NoContent);
                }

            }
            catch (Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Like a reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> LikeReportage(ApplicationDbContext dbContext, long Id)
        {
            try
            {
                var reportage = dbContext.Reportages.Find(Id);
                if (reportage != null)
                {
                    reportage.Likes = reportage.Likes + 1;
                    dbContext.Reportages.Update(reportage);
                    dbContext.SaveChanges();
                    return new APIResponseResult<object>(reportage, HttpStatusCode.OK);
                }
                return new APIResponseResult<object>(null, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Update an existing reportage
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="Id"></param>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public async Task<APIResponseResult<object>> UpdateReportage(ApplicationDbContext dbContext, long Id, string Title, string Description)
        {
            try
            {
                Reportage myreportage = new Reportage()
                {
                    ReportageId = Id,
                    Title = Title,
                    Description = Description,
                    Likes = 0

                };
                if (dbContext.Reportages.Find(Id) != null)
                {
                    dbContext.Reportages.Update(myreportage);
                    dbContext.SaveChanges();     
                }
                else
                {
                    dbContext.Reportages.Add(myreportage);
                    dbContext.SaveChanges();
                }
                return new APIResponseResult<object>(null, HttpStatusCode.OK);

            }
            catch(Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "something went wrong!"
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }
    }
}
