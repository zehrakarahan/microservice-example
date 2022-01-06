using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectIServcie;

namespace AcrossProBatchRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITestService _testService;
        public ValuesController(ITestService testService)
        {
            _testService = testService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            var data = _testService.GetStr();
            return data;
        }

    }
}
