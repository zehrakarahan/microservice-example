using System;
using System.Collections.Generic;
using StructureMap;

namespace XUnitTestProject1
{
    // TODO: Remove `CallContext`, and move to `Sofa.DependencyInjection.Abstractions` assembly.
    // Compatible for ServiceFactory
    // Use IServiceContainer replace IContainer
    //[Obsolete("Use constructor dependency injection.")]
    public static class ServiceFactory
    {
        private const string _callContextName = "IocContainer";
        private static IContainer _container = null;

        public static IContainer GetContainer()
        {
            // Code from ServiceFactory.GetContainer()
            // It is for Job Scheduler project, need multiple Containers. 
            var container = CallContext.GetData(_callContextName) as IContainer;
            container = container ?? _container;
            if (container == null)
            {
                throw new Exception($"No found container from CallContext and static instance.");
            }
            return container;
        }

        internal static void ClearContainer()
        {
            _container = null;
            CallContext.SetData(_callContextName, _container);
        }

        public static void SetContainer(IServiceContainer serviceContainer)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException(nameof(serviceContainer));
            }
            var structureMapServiceContainer = serviceContainer as StructureMapServiceContainer;
            if (structureMapServiceContainer == null)
            {
                throw new InvalidCastException($"Can not cast {serviceContainer.GetType().FullName} to {nameof(StructureMapServiceContainer)}.");
            }

            _container = structureMapServiceContainer.StructureMapContainer;
            CallContext.SetData(_callContextName, _container);
        }

        public static T Get<T>()
        {
            return GetContainer().GetInstance<T>();
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return GetContainer().GetAllInstances<T>();
        }

        public static T Get<T>(string instanceKey)
        {
            return GetContainer().GetInstance<T>(instanceKey);
        }
    }
}