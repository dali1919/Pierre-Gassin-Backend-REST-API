using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebsiteRESTAPI.Entity;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;

namespace WebsiteRESTAPI.Controllers
{
    [Authorize]
    [Route("/[controller]/")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var result = await _userService.Authenticate(model);

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

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _userService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user.RefreshTokens);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Resgister([FromBody] User model)
        {
            try
            {
                var result = await _userService.Register(model);
                switch (result.Status)
                {
                    case HttpStatusCode.OK:
                        return this.Ok(result.Entity);
                    case HttpStatusCode.Found:
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
        // helper methods
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }

}
