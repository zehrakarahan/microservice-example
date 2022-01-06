using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hangfireApp.TimesTask
{
    /// <summary>
    ///  定时任务
    /// </summary>
    class MyJob1 : HangfireWorkerPxoxy,IBackgroundWorkerDo
    {
        private readonly ILogger _logger;
      
        public MyJob1(IConfiguration _appConfiguration, ILogger<MyJob1> logger) : base(_appConfiguration, new WorkerConfig { IntervalSecond = 60 * 1, WorkerId = "RealWorker", QueuesName = "queue1" })
        {
            _logger = logger;
        }

        /// <summary>
        ///  添加同步sku库存任务
        /// </summary>
        public  async Task DoWorkAsync(PerformContext context)
        {
            var temp = await Task.FromResult(0);
            if (context == null)
            {
                return ;
            }
            _logger.LogInformation($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 开始添加同步数据任务 ...");
           
            Thread.Sleep(1000*10);
            _logger.LogInformation($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 结束添加同步数据任务 ...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var temp= await Task.FromResult(0);
            Excete<MyJob1>();
        }

        
    }
}
