using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OneToManyRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        //工厂方式注入
        private readonly Func<string, IService> _serviceAccessor;
        private readonly IService _serviceA;
        private readonly IService _serviceB;
        public FactoryController(Func<string, IService> serviceAccessor)
        {
            _serviceAccessor = serviceAccessor;
            _serviceA = _serviceAccessor("MultiImpDemoA");
            _serviceB = _serviceAccessor("MultiImpDemoB");
        }

        [HttpGet("testA")]
        public IActionResult IndexA()
        {
            var data = _serviceA.GetStr();
            return Content(data);
        }

        [HttpGet("testB")]
        public IActionResult IndexB()
        {
            var data = _serviceB.GetStr();
            return Content(data);
        }
    }
}