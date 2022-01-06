using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedLockApp
{
    public class ProductService
    {
        // 有10个商品库存，如果同时启动多个API服务进行测试，这里改成存数据库或其他方式
        private static int stockCount = 10;
        public async Task<bool> BuyAsync()
        {
            // 模拟执行的逻辑代码花费的时间
            await Task.Delay(new Random().Next(100, 500));
            await Task.Delay(60*1000);
            if (stockCount > 0)
            {
                stockCount--;
                return true;
            }
            return false;
        }
    }
}
