using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace AuthorizeCenter
{
    public class TokenAuthenticationAttribute : ActionFilterAttribute
    {


        /// <summary>
        /// token认证过滤器
        /// </summary>
        /// <param name="loginModel">登录类型</param>
        public TokenAuthenticationAttribute()
        {
    
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            EnforceAuthorization(filterContext);
            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// token验证
        /// </summary>
        /// <param name="actionContext"></param>
        private void EnforceAuthorization(ActionExecutingContext filterContext)
        {
            string _requestUrl = "http://localhost:8000";
            string Bearer = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(Bearer))
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new ObjectResult(null);
                return;
            }
            string applicationUrl = _requestUrl;
            string url = $"{applicationUrl}/AuthorizeCenter";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.Headers.Add("Authorization", Bearer);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                filterContext.HttpContext.Response.StatusCode = (int)response.StatusCode;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    filterContext.Result = new ObjectResult(null);
                }
            }
            //验证失败返回500，强行改成401
            catch (Exception ex)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new ObjectResult(null);
                return;
            }
        }




       

    }
}
