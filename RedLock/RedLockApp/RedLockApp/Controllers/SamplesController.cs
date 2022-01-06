using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedLockNet;

namespace RedLockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly IDistributedLockFactory _distributedLockFactory;
        private readonly ProductService _productService;
        public SamplesController(IDistributedLockFactory distributedLockFactory, ProductService productService)
        {
            _distributedLockFactory = distributedLockFactory;
            _productService = productService;
        }

        [HttpGet]
        public async Task<bool> DistributedLockTest()
        {
            var productId = "id";
            // resource 锁定的对象
            // expiryTime 锁定过期时间，锁区域内的逻辑执行如果超过过期时间，锁将被释放
            // waitTime 等待时间,相同的 resource 如果当前的锁被其他线程占用,最多等待时间
            // retryTime 等待时间内，多久尝试获取一次
            using (var redLock = await _distributedLockFactory.CreateLockAsync(productId, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(20)))
            {
                if (redLock.IsAcquired)
                {
                    var result = await _productService.BuyAsync();
                    return result;
                }
                else
                {
                    Console.WriteLine($"获取锁失败：{DateTime.Now}");
                }
            }
            return false;
        }
    }
}