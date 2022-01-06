using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace My.StackExchange.Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //string
            CacheManagerFactory.GetCacheManager().Add("string","Test1");
            CacheManagerFactory.GetCacheManager().Add("stringWithExpire","Test1");
            var stringWithExpire =  CacheManagerFactory.GetCacheManager().Get<string>("stringWithExpire");

            //hash
            CacheManagerFactory.GetCacheManager().AddToHashList("hash", "Test1","Test2");

            //set
            CacheManagerFactory.GetCacheManager().SetAdd("set", "Test1");
            CacheManagerFactory.GetCacheManager().SetAddRange("set", new List<string>() { "Test2" });
            var set= CacheManagerFactory.GetCacheManager().GetSet<string>("set");

            //list
            CacheManagerFactory.GetCacheManager().EnqueueItem("list","Test2");
            CacheManagerFactory.GetCacheManager().EnqueueItem("list","Test1");
            CacheManagerFactory.GetCacheManager().EnqueueItem("list","Test3");
            CacheManagerFactory.GetCacheManager().BlockingDequeue("list");
            CacheManagerFactory.GetCacheManager().RemoveList<string>("list", "Test2");
            var list = CacheManagerFactory.GetCacheManager().GetListCount("list");


            
            return new string[] { stringWithExpire, set.Count().ToString(), list.ToString()};
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
