using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneToManyRegister.A;
using OneToManyRegister.B;

namespace OneToManyRegister
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

            #region 这样注入如果注入单个，只能获取ServiceB，可以使用数组的注入方式来获取两个
            services.AddTransient<IService, ServiceA>();
            services.AddTransient<IService, ServiceB>();
            #endregion


            #region 工厂注入
            services.AddTransient<ServiceA>();
            services.AddTransient<ServiceB>();
            //工厂注入
            //services.AddTransient(implementationFactory =>
            //{
            //    Func<string, IService> accesor = key =>
            //    {
            //        if (key.Equals("MultiImpDemoA"))
            //        {
            //            return implementationFactory.GetService<ServiceA>();
            //        }
            //        else if (key.Equals("MultiImpDemoB"))
            //        {
            //            return implementationFactory.GetService<ServiceB>();
            //        }
            //        else
            //        {
            //            throw new ArgumentException($"Not Support key : {key}");
            //        }
            //    };
            //    return accesor;

            //});
            services.AddTransient(test);
            #endregion

        }

        private Func<string, IService> test(IServiceProvider implementationFactory)
        {
            Func<string, IService> accesor = key =>
            {
                if (key.Equals("MultiImpDemoA"))
                {
                    return implementationFactory.GetService<ServiceA>();
                }
                else if (key.Equals("MultiImpDemoB"))
                {
                    return implementationFactory.GetService<ServiceB>();
                }
                else
                {
                    throw new ArgumentException($"Not Support key : {key}");
                }
            };
            return accesor;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
