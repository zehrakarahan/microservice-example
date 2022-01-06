using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OneToManyRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IService _serviceA;
        private readonly IService _serviceB;

        
        //数组方式注入
        public ValuesController(IEnumerable<IService> serviceAList)
        {
            _serviceA = serviceAList.FirstOrDefault(h => h.GetType().Namespace == "OneToManyRegister.A");
            _serviceB = serviceAList.FirstOrDefault(h => h.GetType().Namespace == "OneToManyRegister.B");


        }

        [HttpGet("testa")]
        public IActionResult Index()
        {
            var data = _serviceA.GetStr();
            return Content(data);
        }

        [HttpGet("testb")]
        public IActionResult Indexb()
        {
            var data = _serviceB.GetStr();
            return Content(data);
        }
    }
}
