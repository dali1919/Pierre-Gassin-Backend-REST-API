using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public UserService(
            ApplicationDbContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<APIResponseResult<object>> Authenticate(AuthenticateModel model)
        {
            try
            {
                // get user with same user name and password
                if ((string.IsNullOrEmpty(model.Username) && string.IsNullOrEmpty(model.Email)) || string.IsNullOrEmpty(model.Password))
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Missing informations"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                }
                var _user = _context.Users.SingleOrDefault(x =>( x.UserName == model.Username || x.Email == model.Email));
                //decrypt password and compare if entered credentials are ok
                if (_user != null)
                {
                    var decryptedpass = ApiHelper.DecryptStringAES(_user.Password);
                    var user = _context.Users.SingleOrDefault(x =>(( x.UserName == model.Username && decryptedpass == model.Password) || (x.Email == model.Email && decryptedpass == model.Password)));

                    // return null if user not found
                    if (user == null)
                    {
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = false,
                            message = "Invalid Credentials"
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);

                    }
                    else
                    {
                       //authentication successful so generate jwt and refresh tokens
                       // var jwtToken = generateJwtToken(user);
                       // var refreshToken = generateRefreshToken(ipAddress);
                       // save refresh token
                       // user.RefreshTokens.Add(refreshToken);
                        _context.Update(user);
                        _context.SaveChanges();

                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = true,
                            message = "Logged in successfully",
                            data = JsonConvert.SerializeObject(user)
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);

                    }




                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "User not found"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong",
                    data = null
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
          
            
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Password.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        public async Task<APIResponseResult<object>> Register(User model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.FirstName)  || string.IsNullOrEmpty(model.Email) ||
                    string.IsNullOrEmpty(model.Password )|| string.IsNullOrEmpty(model.LastName) ||
                    string.IsNullOrEmpty(model.Country))
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Missing informations"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                }
                // check if a user is already configured with same model
                var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user != null)
                {
                    // You are already registered in our platform login ??
                    // a user with same Email or Name is already configured want to login ??
                    // return status code 
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Account already exists"
                    };
                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);     
                }
                else
                {
                    var user1 = _context.Users.SingleOrDefault(x => x.UserName == model.UserName);
                    if (user1 != null)
                    {
                        // return name exists in our platform
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = false,
                            message = "Account already exists"
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                    }
                    else
                    {
                        // encrypt password and update Database
                        model.Password = ApiHelper.EncryptStringAes(model.Password);
                        _context.Users.Add(model);
                        _context.SaveChanges();
                        model.Password = null;
                        ApiResonse apiResonse = new ApiResonse()
                        {
                            success = true,
                            message = "Successfully created!",
                            data =JsonConvert.SerializeObject( model)
                        };
                        return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                        
                    }
                }
            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong",
                    data = null
                };
                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
            
        }
    }

}
