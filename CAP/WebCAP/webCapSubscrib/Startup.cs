using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace webCapSubscrib
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<AppDbContext>(options => options.UseMySql(Configuration.GetConnectionString("SqlConnectionStr"))); ;
            services.AddCap(x =>
            {
                x.UseEntityFramework<AppDbContext>();

                ////使用Dapper ORM
                //x.UseMySql(Configuration.GetConnectionString("DBConnection"));

                //使用kafka 进行日志、case的消息推送
                //需要配置一下MQ地址，kafka放在linux系统上，不建议放在window上
                x.UseRabbitMQ(options =>
                {
                    options.HostName = "192.168.0.102";
                    options.UserName = "guest";
                    options.Password = "guest";
                });
                x.UseDashboard();//得到UI界面

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCap();
            app.UseMvc();
        }
    }
}
