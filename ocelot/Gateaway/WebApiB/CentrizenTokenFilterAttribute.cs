using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiB
{
    public class CentrizenTokenFilterAttribute : ActionFilterAttribute
    {




        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var test = filterContext.HttpContext.Request.Path;
            string bearer = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(bearer) || !bearer.Contains("Bearer")) return;
            string[] jwt = bearer.Split(' ');
            var tokenObj = new JwtSecurityToken(jwt[1]);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(tokenObj.Claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = claimsPrincipal;
            filterContext.HttpContext.User = claimsPrincipal;


        }

    }
}