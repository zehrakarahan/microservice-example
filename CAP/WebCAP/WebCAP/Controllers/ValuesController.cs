using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebCAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICapPublisher _capBus;
        private readonly AppDbContext _db;
        public ValuesController(ICapPublisher capBus, AppDbContext db)
        {
            _capBus = capBus;
            _db = db;
        }
        // GET api/values
        [HttpGet]
        public void Get()
        {
     
                _capBus.Publish("xxx.services.name", "name", "xxx.services.name");
            
            
            //_capBus.Publish("xxx.services.show.time", DateTime.Now, "xxx.services.show.time");
        }

        

        [HttpGet("Test")]
        [CapSubscribe("xxx.services.show.time")]
        public async Task<Task> CheckReceivedMessage(DateTime time)
        {
            Console.WriteLine(time);
            return Task.CompletedTask;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
