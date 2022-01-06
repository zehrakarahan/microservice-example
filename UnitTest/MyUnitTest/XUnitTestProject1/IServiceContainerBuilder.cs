using System;
using System.Collections.Generic;

namespace XUnitTestProject1
{
    public interface IServiceContainerBuilder
    {
        IServiceContainerCreator ServiceContainerCreator { get; set; }
        IList<IServiceRegistry> Registries { get; }
        IServiceContainer Build();
        event Action<IServiceContainer> BuildCompeleted;
    }
}