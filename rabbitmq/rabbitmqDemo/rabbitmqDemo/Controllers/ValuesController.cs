using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rabbitmq.Common;
using Rabbitmq.Common.BaseEvent;
using Rabbitmq.Common.CustormEvent;

namespace rabbitmqDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEventBus eventBus;
        public ValuesController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            await this.eventBus.PublishAsync(new CustomerCreatedEvent<string>("test"));
            await this.eventBus.PublishAsync(new CustomerCreatedEvent<TestLib1>(new TestLib1 { Name="xa",Age=20}));
            await this.eventBus.PublishAsync(new CustomerCreatedEvent<TestLib2>(new TestLib2 { Addr = "xb", Prize = 21 }));
            return new string[] { "value1", "value2" };
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
