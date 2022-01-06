using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rabbitmq.Common;
using Rabbitmq.Common.BaseEvent;
using Rabbitmq.Common.CustormEvent;
using RabbitMQ.Client;
using Rbbitmq.Common.ExecutionContext;

namespace RabbitmqConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private const string RMQ_EXCHANGE = "Demo.Exchange";
        private const string RMQ_QUEUE = "TestQueName";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IEventStore>(serviceProvider =>
             new DapperEventStore(Configuration["mssql:connectionString"],
                 serviceProvider.GetRequiredService<ILogger<DapperEventStore>>()));
            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services,
               sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);

            var connectionFactory = new ConnectionFactory { HostName = "192.168.0.102", UserName = "guest", Password = "guest", Port = 5672 };
            services.AddSingleton<IEventBus>(sp => new RabbitMQEventBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                sp.GetRequiredService<IEventHandlerExecutionContext>(),
                RMQ_EXCHANGE,
                queueName: RMQ_QUEUE, subscribe:true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CustomerCreatedEvent<TestLib1>, CustomerCreatedEventHandler<TestLib1>>();
            eventBus.Subscribe<CustomerCreatedEvent<TestLib2>, CustomerCreatedEventHandler<TestLib2>>();
            app.UseMvc();
        }
    }
}
