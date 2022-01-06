using Hangfire;
using Hangfire.Common;
using Hangfire.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hangfireApp.TimesTask
{
    public abstract class HangfireWorkerPxoxy:BackgroundService
    {
        private readonly IConfiguration _appConfiguration;
        private readonly WorkerConfig _config;
        public HangfireWorkerPxoxy(IConfiguration appConfiguration, WorkerConfig Config)
        {
            _appConfiguration = appConfiguration;
            _config = Config;
        }
        
        public void Excete<T>() where T : IBackgroundWorkerDo
        {
            var appsetting = _appConfiguration.GetSection("Appsetting").Get<Appsetting>();
            var redisString = appsetting.RedisConfig.HostName + ":" + appsetting.RedisConfig.Port + ",defaultdatabase="+ appsetting.RedisConfig.Defaultdatabase;
            JobStorage.Current = new RedisStorage(redisString);
            var manager = new RecurringJobManager(JobStorage.Current);
            string workerId = _config.WorkerId;
            string cron = string.IsNullOrEmpty(_config.Cron) ? Cron.MinuteInterval(_config.IntervalSecond / 60) : _config.Cron;
            manager.RemoveIfExists(workerId);
            manager.AddOrUpdate(workerId, Job.FromExpression<T>((t) => t.DoWorkAsync(null)), cron, TimeZoneInfo.Local, string.IsNullOrEmpty(_config.QueuesName) ? "default" : _config.QueuesName);
        }

        
    }
}
