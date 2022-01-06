using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassLibrary1;
using Microsoft.AspNetCore.Mvc;

namespace AutomapperAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DepartmentDTO sampleDepartment;
        public ValuesController(IMapper mapper)
        {
            _mapper = mapper;
            sampleDepartment = new DepartmentDTO
            {
                Name = "department1",
                Id = 1,
                Owner = "ABC"
                //SecretProperty = "Very secret property"
            };


        }

        [HttpGet]
        public ActionResult<Department> Index()
        {
            byte a = 100;
            int t = Convert.ToInt32(a);
            return _mapper.Map<Department>(sampleDepartment);
        }


        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

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
