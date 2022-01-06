using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace RedLockApp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(typeof(IDistributedLockFactory), lockFactory);
            services.AddScoped(typeof(ProductService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            lifetime.ApplicationStopping.Register(() =>
            {
                lockFactory.Dispose();
            });
        }



        private RedLockFactory lockFactory
        {
            get
            {
                var redisUrls = Configuration.GetSection("RedisUrls").GetChildren().Select(s => s.Value).ToArray();
                if (redisUrls.Length <= 0)
                {
                    throw new ArgumentException("RedisUrl 不能为空");
                }
                var endPoints = new List<RedLockEndPoint>();
                foreach (var item in redisUrls)
                {
                    var arr = item.Split(":");
                    endPoints.Add(new DnsEndPoint(arr[0], Convert.ToInt32(arr[1])));
                }
                return RedLockFactory.Create(endPoints);
            }
        }
    }
}
