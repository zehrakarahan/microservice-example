using System;
using System.Threading.Tasks;

namespace Rabbitmq.Common.BaseEvent
{
    public interface IEventStore : IDisposable
    {
        Task SaveEventAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}