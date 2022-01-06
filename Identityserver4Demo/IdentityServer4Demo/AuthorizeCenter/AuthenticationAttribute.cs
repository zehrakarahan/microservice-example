//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;

//namespace AuthorizeCenter
//{
//    public class AuthenticationAttribute : ActionFilterAttribute
//    {


//        /// <summary>
//        /// token认证过滤器
//        /// </summary>
//        /// <param name="loginModel">登录类型</param>
//        public AuthenticationAttribute()
//        {
    
//        }


//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
           
//                EnforceAuthorization(filterContext);
           
//            base.OnActionExecuting(filterContext);
//        }




//        /// <summary>
//        /// 添加用户数据到ClaimsIdentity，为扩展自定义IAbpSession准备数据
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="userName"></param>
//        /// <param name="allCompany"></param>
//        /// <param name="companyId"></param>
//        private void CreateIdentity(string userId, string userName, string allCompany, string companyId, string rolecodes, HttpContext HttpContext)
//        {
//            var claimsIdentity = new ClaimsIdentity();
//            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String));
//            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String));
//            claimsIdentity.AddClaim(new Claim("IsAllCompany", allCompany, ClaimValueTypes.String));
//            claimsIdentity.AddClaim(new Claim("CompanyId", companyId, ClaimValueTypes.String));
//            claimsIdentity.AddClaim(new Claim("rolesCode", rolecodes, ClaimValueTypes.String));
//            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
//            Thread.CurrentPrincipal = claimsPrincipal;
//            HttpContext.User = claimsPrincipal;
//            HttpContext.SignInAsync(claimsPrincipal);
//        }


//        /// <summary>
//        /// token验证
//        /// </summary>
//        /// <param name="actionContext"></param>
//        private void EnforceAuthorization(ActionExecutingContext filterContext)
//        {
//            APIResult<object> contentResult = new APIResult<object>()
//            {
//                Message = "无权限访问地址！",
//                Code = (int)HttpStatusCode.Unauthorized
//            };
//            string _requestUrl = ConfigManagerConf.GetValue("AppSetting:SsoUrl");
//            ////********验证token成功返回资源信息，验证失败返回空
//            string Bearer = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
//            if (string.IsNullOrEmpty(Bearer))
//            {
//                filterContext.Result = new ObjectResult(contentResult);
//                return;
//            }
//            string applicationUrl = _requestUrl;
//            string url = $"{applicationUrl}/GetUserResource";
//            string resourceJson = "";
//            Encoding encoding = Encoding.UTF8;
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "get";
//            request.Headers.Add("Authorization", Bearer);
//            try
//            {
//                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
//                {
//                    resourceJson = reader.ReadToEnd();
//                    JObject obj = JObject.Parse(resourceJson);

//                    var result = obj["result"];
//                    string userId = result["UserId"].ToString();
//                    string userName = result["Account"].ToString();
//                    string allCompany = result["AllCompany"].ToString();
//                    var Company = result["Company"].ToString();
//                    JObject objCompany = JObject.Parse(Company);
//                    var roles = result["roles"].ToString();
//                    List<RoleModel> jobRoleDtos = JsonConvert.DeserializeObject<List<RoleModel>>(roles);
//                    var roleCodes = jobRoleDtos.Select(p => p.RoleCode).ToList();
//                    string rolecodes = "";
//                    if (roleCodes.Count > 0) rolecodes = string.Join(",", roleCodes.ToArray());
//                    var companyId = objCompany["Id"].ToString();
//                    CreateIdentity(userId, userName, allCompany, companyId, rolecodes, filterContext.HttpContext);

//                }
//            }
//            //验证失败返回500，强行改成401
//            catch (Exception ex)
//            {
//                filterContext.Result = new ObjectResult(contentResult);
//                return;
//            }
//        }




//        /// <summary>
//        /// public 外部调用账号验证
//        /// </summary>
//        /// <param name="actionContext"></param>
//        private void EnforcePublicAuthorization(ActionExecutingContext filterContext)
//        {
//            APIResult contentResult = new APIResult()
//            {
//                Message = "无权限访问地址！",
//                StatusCode = (int)HttpStatusCode.Unauthorized
//            };
//            string _requestUrl = ConfigManagerConf.GetValue("AppSetting:SsoUrl");

//            ////********验证token成功返回资源信息，验证失败返回空
//            string token = filterContext.HttpContext.Request.Headers["token"].FirstOrDefault();
//            string account = filterContext.HttpContext.Request.Headers["account"].FirstOrDefault();
//            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(account))
//            {
//                filterContext.Result = new ObjectResult(contentResult);
//                return;
//            }
//            string url = $"{_requestUrl}/PublicTokenValidation";
//            string resourceJson = "";
//            Encoding encoding = Encoding.UTF8;
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "get";
//            request.Headers.Add("token", token);
//            request.Headers.Add("account", account);
//            try
//            {
//                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
//                {
//                    resourceJson = reader.ReadToEnd();
//                    JObject obj = JObject.Parse(resourceJson);

//                    var result = obj["result"];
//                    if (result.ToBoolean())
//                    {
//                        string userId = "0";
//                        string userName = "system";
//                        string allCompany = "true";
//                        string companyId = "1";
//                        string rolecode = "pps_publish";
//                        CreateIdentity(userId, userName, allCompany, companyId, rolecode, filterContext.HttpContext);
//                    }
//                    else
//                    {
//                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                        filterContext.Result = new EmptyResult();
//                        return;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                filterContext.Result = new ObjectResult(contentResult);
//                return;
//            }
//        }


//        /// <summary>
//        /// token验证,没token默认用百伦的用户
//        /// </summary>
//        /// <param name="actionContext"></param>
//        private void IgEnforceAuthorization(ActionExecutingContext filterContext)
//        {
//            APIResult contentResult = new APIResult()
//            {
//                Message = "无权限访问地址！",
//                StatusCode = (int)HttpStatusCode.Unauthorized
//            };
//            string _requestUrl = ConfigManagerConf.GetValue("AppSetting:SsoUrl");
//            ////********验证token成功返回资源信息，验证失败返回空
//            string Bearer = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
//            if (string.IsNullOrEmpty(Bearer))
//            {
//                string userId = "0";
//                string userName = "systemIg";
//                string allCompany = "false";
//                string companyId = "1";
//                string rolecodes = "pps_publish";
//                CreateIdentity(userId, userName, allCompany, companyId, rolecodes, filterContext.HttpContext);
//                return;
//            }
//            string applicationUrl = _requestUrl;
//            string url = $"{applicationUrl}/GetUserResource";
//            string resourceJson = "";
//            Encoding encoding = Encoding.UTF8;
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "get";
//            request.Headers.Add("Authorization", Bearer);
//            try
//            {
//                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
//                {
//                    resourceJson = reader.ReadToEnd();
//                    JObject obj = JObject.Parse(resourceJson);

//                    var result = obj["result"];
//                    string userId = result["UserId"].ToString();
//                    string userName = result["Account"].ToString();
//                    string allCompany = result["AllCompany"].ToString();
//                    var Company = result["Company"].ToString();
//                    JObject objCompany = JObject.Parse(Company);
//                    var roles = result["roles"].ToString();
//                    List<RoleModel> jobRoleDtos = JsonConvert.DeserializeObject<List<RoleModel>>(roles);
//                    var roleCodes = jobRoleDtos.Select(p => p.RoleCode).ToList();
//                    string rolecodes = "";
//                    if (roleCodes.Count > 0) rolecodes = string.Join(",", roleCodes.ToArray());
//                    var companyId = objCompany["Id"].ToString();
//                    CreateIdentity(userId, userName, allCompany, companyId, rolecodes, filterContext.HttpContext);

//                }
//            }
//            //验证失败返回500，强行改成401
//            catch (Exception ex)
//            {
//                filterContext.Result = new ObjectResult(contentResult);
//                return;
//            }
//        }

//    }
//}
