using System;
using System.Collections.Generic;

namespace XUnitTestProject1
{
    public class ServiceContainerBuilder : IServiceContainerBuilder
    {
        public IServiceContainerCreator ServiceContainerCreator { get; set; }
        public IList<IServiceRegistry> Registries { get; }
        public event Action<IServiceContainer> BuildCompeleted;

        public ServiceContainerBuilder()
        {
            Registries = new List<IServiceRegistry>();
        }

        public IServiceContainer Build()
        {
            if (ServiceContainerCreator == null)
            {
                throw new Exception();
            }
            var container = ServiceContainerCreator.Create(Registries);
            BuildCompeleted?.Invoke(container);

            return container;
        }
    }
}
