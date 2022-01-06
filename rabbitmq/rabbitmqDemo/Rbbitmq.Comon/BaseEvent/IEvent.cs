using System;

namespace Rabbitmq.Common.BaseEvent
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}