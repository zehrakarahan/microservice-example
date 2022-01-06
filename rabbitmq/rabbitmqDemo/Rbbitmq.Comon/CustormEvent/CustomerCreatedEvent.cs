using Rabbitmq.Common.BaseEvent;
using System;

namespace Rabbitmq.Common.CustormEvent
{
    public class CustomerCreatedEvent<T> : IEvent
    {
        public CustomerCreatedEvent(T customerName)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            this.CustomerName = customerName;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public T CustomerName { get; }
    }
}
