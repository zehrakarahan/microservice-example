using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hangfireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var jobId = await System.Threading.Tasks.Task.Run(() => BackgroundJob.Enqueue(
     () => PostAsync()));
            return jobId;
        }

        // POST api/values
        [HttpPost]
        [Queue("default")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task PostAsync()
        {
            var t = await Task.FromResult(0);
            _logger.LogInformation("正在执行。。。");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

       

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
