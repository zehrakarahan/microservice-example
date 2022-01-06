using Microsoft.Extensions.Logging;
using Rabbitmq.Common.BaseEvent;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitmq.Common.CustormEvent
{
    public class CustomerCreatedEventHandler<T> : IEventHandler<CustomerCreatedEvent<T>>
    {
        private readonly IEventStore eventStore;
        private readonly ILogger logger;

        public CustomerCreatedEventHandler(IEventStore eventStore,
            ILogger<CustomerCreatedEventHandler<T>> logger)
        {
            this.eventStore = eventStore;
            this.logger = logger;
            this.logger.LogInformation($"CustomerCreatedEventHandler构造函数调用完成。Hash Code: {this.GetHashCode()}.");
        }

        public bool CanHandle(IEvent @event)
            => @event.GetType().Equals(typeof(CustomerCreatedEvent<T>));

        public async Task<bool> HandleAsync(CustomerCreatedEvent<T> @event, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation($"开始处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            await this.eventStore.SaveEventAsync(@event);
            this.logger.LogInformation($"结束处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            return true;
        }

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
            => CanHandle(@event) ? HandleAsync((CustomerCreatedEvent<T>)@event, cancellationToken) : Task.FromResult(false);
    }
}
