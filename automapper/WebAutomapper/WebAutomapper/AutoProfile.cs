using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAutomapper
{
    public class AutoProfile: Profile
    {
        public AutoProfile()
        {
            //定义映射，ReverseMap表示双向，还可以自定义映射字段
            CreateMap<Student, StudentDto>().ReverseMap();
        }
       
    }
}
