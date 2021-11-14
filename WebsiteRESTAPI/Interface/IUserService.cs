using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Interface
{
    public interface IUserService
    {
        Task<APIResponseResult<object>> Authenticate(AuthenticateModel model);
        Task<APIResponseResult<object>> Register(User model);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
