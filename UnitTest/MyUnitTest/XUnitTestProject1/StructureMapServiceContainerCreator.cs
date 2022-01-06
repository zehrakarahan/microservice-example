using System;
using System.Collections.Generic;
using StructureMap;

namespace XUnitTestProject1
{
    internal class StructureMapServiceContainerCreator: IServiceContainerCreator
    {
        public IServiceContainer Create(IList<IServiceRegistry> registries)
        {
            var container = new Container(x =>
            {
                AddRegistries(x, registries);
            });

            var serviceContainer = new StructureMapServiceContainer(container);
            return serviceContainer;
        }

        private void AddRegistries(ConfigurationExpression x, IList<IServiceRegistry> registries)
        {
            foreach (var registry in registries)
            {
                var realRegistry = registry as global::StructureMap.Registry;
                if (realRegistry == null)
                {
                    throw new InvalidCastException($"Can not cast {registry.GetType().FullName} to {nameof(global::StructureMap.Registry)}.");
                }
                x.AddRegistry(realRegistry);
            }
        }
    }
}
