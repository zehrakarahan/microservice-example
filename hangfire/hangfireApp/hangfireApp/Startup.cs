using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Redis;
using hangfireApp.TimesTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace hangfireApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appsetting = Configuration.GetSection("Appsetting").Get<Appsetting>();
            var redisStorage = new RedisStorageOptions
            {
                Db = appsetting.RedisConfig.Defaultdatabase,
                SucceededListSize = 50000,
                DeletedListSize = 50000,
                ExpiryCheckInterval = TimeSpan.FromHours(1),
                InvisibilityTimeout = TimeSpan.FromHours(3),
            };
            var redisString = appsetting.RedisConfig.HostName + ":" + appsetting.RedisConfig.Port;
            services.AddHangfire(x =>
            {
                x.UseRedisStorage(redisString, redisStorage);
            });
            services.AddHostedService<MyJob1>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appsetting = Configuration.GetSection("Appsetting").Get<Appsetting>();
            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = appsetting.TaskQueues.ToArray(),//队列名称，只能为小写
                WorkerCount = Environment.ProcessorCount * 5, //并发任务数
                ServerName = "MyServer",//服务器名称
            };
            app.UseHangfireServer(jobOptions);
            app.UseHangfireDashboard();
            app.UseMvc();
        }
    }
}
