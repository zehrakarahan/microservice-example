using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WebConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test1Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigModel _configModel;
        public Test1Controller(IConfiguration configuration,
             IOptions<ConfigModel> options)
        {
            _configuration = configuration;
            _configModel = options.Value;
        }

        //推荐方式
        [HttpGet("Test3")]
        public string Test3()
        {
            var configModel = _configuration.GetSection("Position").Get<ConfigModel>();
            var temp = _configuration.GetSection("MyKey").Get<string>();
            return ($"Title: {configModel.Title} \n" +
                           $"Name: {configModel.Name}");
        }

        //推荐方式
        [HttpGet("Test4")]
        public string Test4()
        {
            return ($"Title: {_configModel.Title} \n" +
                           $"Name: {_configModel.Name}");
        }

        [HttpGet("Test")]
        public string Test()
        {
            var myKeyValue = _configuration["MyKey"];
            var title = _configuration["Position:Title"];
            var name = _configuration["Position:Name"];
            var defaultLogLevel = _configuration["Logging:LogLevel:Default"];


            return ($"MyKey value: {myKeyValue} \n" +
                           $"Title: {title} \n" +
                           $"Name: {name} \n" +
                           $"Default Log Level: {defaultLogLevel}");
        }

        [HttpGet("Test2")]
        public string Test2()
        {
            var configModel = new ConfigModel();
            _configuration.GetSection("Position").Bind(configModel);
            return ($"Title: {configModel.Title} \n" +
                           $"Name: {configModel.Name}");
        }

    }
}