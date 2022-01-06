using System;
using System.Data;
using CodeGenerator.CodeGenerator.CodeFirst;
using CodeGenerator.CodeGenerator.DbFirst;
using CodeGenerator.Models;
using CodeGenerator.Options;
using Microsoft.Extensions.DependencyInjection;
using Zxw.Framework.NetCore.IDbContext;
using Zxw.Framework.NetCore.IoC;

namespace CodeGenerator.CodeGenerator
{
    public static class CodeGeneratorExtenstions
    {
        public static void ToGenerateViewModelFile(this DataTable dt, string className)
        {
            var dbFirst = ServiceLocator.Resolve<IDbFirst>();
            dbFirst.GenerateViewModel(dt, className);
        }

        public static void ToGenerateViewModelFile(this DataSet ds)
        {
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            foreach (DataTable table in ds.Tables)
            {
                table.ToGenerateViewModelFile(table.TableName);
            }
        }

        //public static void GenerateAllCodesFromDatabase(this IDbContextCore dbContext, bool ifExistCovered = false,
        //    Func<DbTable, bool> selector = null)
        //{
        //    dbContext.DbFirst().Generate(selector, ifExistCovered);
        //}

        public static void UseCodeGenerator(this IServiceCollection services, CodeGenerateOption option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            services.Configure<CodeGenerateOption>(config =>
                {
                    config.ControllersNamespace = option.ControllersNamespace;
                    config.IRepositoriesNamespace = option.IRepositoriesNamespace;
                    config.IServicesNamespace = option.IServicesNamespace;
                    config.ModelsNamespace = option.ModelsNamespace;
                    config.OutputPath = option.OutputPath;
                    config.RepositoriesNamespace = option.RepositoriesNamespace;
                    config.ServicesNamespace = option.ServicesNamespace;
                    config.ViewModelsNamespace = option.ViewModelsNamespace;
                    config.IsPascalCase = option.IsPascalCase;
                });
            services.AddSingleton<IDbFirst, DbFirst.DbFirst>();
            services.AddSingleton<ICodeFirst, CodeFirst.CodeFirst>();
        }
    }
}
