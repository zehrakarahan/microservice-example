using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthServiceCentre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        //[HttpPost("Login")]
        //public async Task<JsonResult> LoginAsync([FromBody]AccountInput input)
        //{
        //    var client = new HttpClient();
        //    var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
        //    if (disco.IsError)
        //        new JsonResult(new { Code = 500, Data = disco.Error });
        //    TokenResponse token = null;
        //    token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        //    {
        //        //获取Token的地址
        //        Address = disco.TokenEndpoint,
        //        //客户端Id
        //        ClientId = OAuthConfig.UserApi.ClientId,
        //        //客户端密码
        //        ClientSecret = OAuthConfig.UserApi.Secret,
        //        //要访问的api资源
        //        //Scope = OAuthConfig.UserApi.ApiName,
        //        UserName = input.AccountName,
        //        Password = input.Password,
        //    });
        //    LoginResponse loginResponse = new LoginResponse();
        //    loginResponse.AccessToken = token.AccessToken;
        //    loginResponse.IdentityToken = token.IdentityToken;
        //    loginResponse.TokenType = token.TokenType;
        //    loginResponse.RefreshToken = token.RefreshToken;
        //    loginResponse.ErrorDescription = token.ErrorDescription;
        //    loginResponse.ExpiresIn = token.ExpiresIn;

        //    return new JsonResult(new { Code = 200, Data = loginResponse });
        //}
    }
}
