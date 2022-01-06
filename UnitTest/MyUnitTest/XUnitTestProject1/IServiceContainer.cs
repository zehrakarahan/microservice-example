using System;
using System.Collections;
using System.Collections.Generic;

namespace XUnitTestProject1
{
    public interface IServiceContainer : IDisposable
    {
        T GetInstance<T>();
        T GetInstance<T>(string instanceKey);
        T TryGetInstance<T>();
        object GetInstance(Type pluginType);
        object TryGetInstance(Type pluginType);
        IEnumerable GetAllInstances(Type pluginType);
        IEnumerable<T> GetAllInstances<T>();
        IServiceContainer CreateChildContainer();

        //[Obsolete("Note! Don't do this, you've been warned.")]
        void Configure(IServiceRegistry registry);
    }
}
