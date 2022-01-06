using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGenerator.CodeGenerator;
using CodeGenerator.CodeGenerator.DbFirst;
using CodeGenerator.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zxw.Framework.NetCore.DbContextCore;
using Zxw.Framework.NetCore.IDbContext;

namespace CodeGenerator
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
            services.AddControllers();


            //≈‰÷√DbContextOption
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("mysql");
                options.ModelAssemblyName = "CodeGenerator";
                options.IsOutputSql = true;
            });
            //services.Configure<CodeGenerateOption>(config =>
            //{
            //    config.ControllersNamespace = option.ControllersNamespace;
            //    config.IRepositoriesNamespace = option.IRepositoriesNamespace;
            //    config.IServicesNamespace = option.IServicesNamespace;
            //    config.ModelsNamespace = option.ModelsNamespace;
            //    config.OutputPath = option.OutputPath;
            //    config.RepositoriesNamespace = option.RepositoriesNamespace;
            //    config.ServicesNamespace = option.ServicesNamespace;
            //    config.ViewModelsNamespace = option.ViewModelsNamespace;
            //    config.IsPascalCase = option.IsPascalCase;
            //});
            services.AddScoped<IDbFirst, DbFirst>();
            services.AddScoped<IDbContextCore, MySqlDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
