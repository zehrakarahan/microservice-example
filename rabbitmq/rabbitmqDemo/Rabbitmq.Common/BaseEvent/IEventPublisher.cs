using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitmq.Common.BaseEvent
{
    public interface IEventPublisher : IDisposable
    {
        Task PublishAsync<TEvent>(TEvent @event,string queName, CancellationToken cancellationToken = default)
            where TEvent : IEvent;
    }
}