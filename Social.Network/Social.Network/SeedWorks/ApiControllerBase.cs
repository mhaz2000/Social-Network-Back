using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Network.SeedWorks.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Social.Network.SeedWorks
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {

        protected string AccessToken => Request.Headers.ContainsKey("Authorization")
            ? Request.Headers["Authorization"].ToString().Split(" ")[1]
            : string.Empty;

        protected virtual string UserName => GetClaim<string>(ClaimTypes.GivenName);
        protected virtual Guid UserId => GetClaim<Guid>("id");

        protected ApiControllerBase()
        {
        }

        [NonAction]
        protected virtual IActionResult OkResult()
        {
            return this.OkResult("", null);
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message)
        {
            return this.OkResult(message, null);
        }

        [NonAction]
        protected virtual IActionResult OkContentResult(object content)
        {
            return this.OkResult("", content);
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message, object content)
        {
            return Ok(new ResponseMessage(message, content));
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message, object content, int total)
        {
            return Ok(new ResponseMessage(message, content, total));
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(string message)
        {
            return BadRequest(new { Message = message });
        }

        [NonAction]
        public NotFoundObjectResult NotFound(string message)
        {
            return NotFound(new { Message = message });
        }

        private T GetClaim<T>(string claimType)
        {
            try
            {
                var stream = this.AccessToken;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                var claim = tokenS.Claims.FirstOrDefault(claim => claim.Type == claimType).Value;
                var result = claim.ToType<T>();
                return result;
            }
            catch (System.Exception ex)
            {
                if (this.ControllerContext.HttpContext.User.Claims == null)
                    throw new SystemException("دسترسی نامعتبر، دوباره تلاش کنید.");

                throw new SystemException("دسترسی نامعتبر", ex);
            }
        }

    }
}
