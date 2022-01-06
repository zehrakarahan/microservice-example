using System.Collections.Generic;

namespace XUnitTestProject1
{
    public interface IServiceContainerCreator
    {
        IServiceContainer Create(IList<IServiceRegistry> registries);
    }
}
