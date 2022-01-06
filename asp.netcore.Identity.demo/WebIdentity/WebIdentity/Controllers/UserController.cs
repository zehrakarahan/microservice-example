using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // POST: api/User
        [HttpPost]
        public async Task PostAsync([FromBody]User _user)
        {
            //先创建一个user，不包括密码
            var user = new User { Email = _user.Email, UserName = _user.UserName, Pass = "123456" };
            //将user和密码绑定入库
            var result = await _userManager.CreateAsync(user, user.Pass);
            if (result.Succeeded)
            {
                Console.Write("注册成功！");
            }
        }
    }
}
