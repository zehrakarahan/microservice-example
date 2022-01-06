using System;
using System.Collections;
using System.Collections.Generic;
using StructureMap;

namespace XUnitTestProject1
{
    internal class StructureMapServiceContainer : IServiceContainer
    {
        internal IContainer StructureMapContainer { get; private set; }

        public StructureMapServiceContainer(IContainer container)
        {
            StructureMapContainer = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Dispose()
        {
            StructureMapContainer.Dispose();
        }

        public T GetInstance<T>()
        {
            return StructureMapContainer.GetInstance<T>();
        }

 
        public T GetInstance<T>(string instanceKey)
        {
            return StructureMapContainer.GetInstance<T>(instanceKey);
        }

        public object GetInstance(Type pluginType)
        {
            if (pluginType == null)
            {
                throw new ArgumentNullException(nameof(pluginType));
            }
            return StructureMapContainer.GetInstance(pluginType);
        }

        public T TryGetInstance<T>()
        {
            return StructureMapContainer.TryGetInstance<T>();
        }

        public object TryGetInstance(Type pluginType)
        {
            if (pluginType == null)
            {
                throw new ArgumentNullException(nameof(pluginType));
            }
            return StructureMapContainer.TryGetInstance(pluginType);
        }

        public IEnumerable GetAllInstances(Type pluginType)
        {
            if (pluginType == null)
            {
                throw new ArgumentNullException(nameof(pluginType));
            }
            return StructureMapContainer.GetAllInstances(pluginType);
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return StructureMapContainer.GetAllInstances<T>();
        }

        public IServiceContainer CreateChildContainer()
        {
            return new StructureMapServiceContainer(StructureMapContainer.CreateChildContainer());
        }

        [Obsolete("Note! Don't do this, you've been warned.")]
        public void Configure(IServiceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var realRegistry = registry as global::StructureMap.Registry;
            if (realRegistry == null)
            {
                throw new InvalidCastException($"Can not cast {registry.GetType().FullName} to {nameof(global::StructureMap.Registry)}.");
            }
            StructureMapContainer.Configure(x => x.AddRegistry(realRegistry));
        }
    }
}
