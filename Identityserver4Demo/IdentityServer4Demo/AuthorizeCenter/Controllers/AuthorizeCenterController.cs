using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AuthorizeCenter.Dto;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthorizeCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizeCenterController : ControllerBase
    {


        private readonly ILogger<AuthorizeCenterController> _logger;

        public AuthorizeCenterController(ILogger<AuthorizeCenterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new ContentResult();
        }

        [HttpPost("Login")]
        public async Task<JsonResult> LoginAsync([FromBody]AccountInput input)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
                new JsonResult(new { Code = 500, Data = disco.Error });
            TokenResponse token = null;
            token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                //获取Token的地址
                Address = disco.TokenEndpoint,
                //客户端Id
                ClientId = OAuthConfig.UserApi.ClientId,
                //客户端密码
                ClientSecret =OAuthConfig.UserApi.Secret,
                //要访问的api资源
                //Scope = OAuthConfig.UserApi.ApiName,
                UserName = input.AccountName,
                Password = input.Password,
            });
            LoginResponse loginResponse = new LoginResponse();
            loginResponse.AccessToken = token.AccessToken;
            loginResponse.IdentityToken = token.IdentityToken;
            loginResponse.TokenType = token.TokenType;
            loginResponse.RefreshToken = token.RefreshToken;
            loginResponse.ErrorDescription = token.ErrorDescription;
            loginResponse.ExpiresIn = token.ExpiresIn;

            return new JsonResult(new { Code = 200, Data = loginResponse });
        }
    }
}
