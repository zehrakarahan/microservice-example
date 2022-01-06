using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebAutomapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMapper _mapper;
        public ValuesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public Student Get()
        {
            var studentDto = new StudentDto { Name = "张三", Addr = "广州市", Age = 18, Sex = true };
            var student = _mapper.Map<StudentDto, Student>(studentDto);
            return student;
        }

       
    }
}
